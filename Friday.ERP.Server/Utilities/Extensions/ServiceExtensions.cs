using System.Net.Mime;
using System.Text;
using Friday.ERP.Core.Data.Constants.AppSettings;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Infrastructure;
using Friday.ERP.Infrastructure.Repositories;
using Friday.ERP.Infrastructure.Services;
using Friday.ERP.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;

namespace Friday.ERP.Server.Utilities.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
        });
    }

    public static void ConfigureIisIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(_ => { });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseOracle(configuration.GetConnectionString("SqlConnection")!,
                o =>
                {
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    o.MigrationsAssembly("Friday.ERP.Server");
                    o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19);
                }));
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Friday ERP API",
                Version = "v1",
                Description = "Friday ERP"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer authorisation token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "Bearer {token}",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });
    }

    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        appContext.Database.Migrate();
        return webApp;
    }

    public static void ConfigureApiBehaviorOptions(this IServiceCollection services, Logger logger)
    {
        services.PostConfigure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var validationErrors = context.ModelState.SelectMany(x => x.Value!.Errors)
                    .ToArray();

                var refNo = Guid.NewGuid();
                ErrorResponseDto<string> response = new()
                {
                    ErrorReferenceNumber = refNo,
                    ErrorType = "Validation Error",
                    ErrorDetail = new List<string>()
                };

                if (validationErrors.Any())
                    foreach (var validation in validationErrors)
                        response.ErrorDetail.Add(validation.ErrorMessage);

                var routeValue = context.HttpContext.Request.Path.Value;

                var logText =
                    $"ErrorReferenceNumber : {refNo}. Response : {JsonConvert.SerializeObject(response)}. Path => {routeValue}.";
                logger.Info(logText);
                return new UnprocessableEntityObjectResult(response);
            };
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration();
        configuration.Bind("JwtSettings", jwtConfiguration);

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.JwtSecret!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        var errorResponse = new ErrorResponseDto<string>
                        {
                            ErrorReferenceNumber = Guid.NewGuid(),
                            ErrorType = "Invalid Token",
                            ErrorDetail = new List<string> { "Invalid JWT Token" }
                        };
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                    }
                };
            });
    }
}