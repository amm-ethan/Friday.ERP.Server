using Friday.ERP.Core.IServices.Hubs;
using Friday.ERP.Infrastructure.Services.Hubs;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Server.Hubs;
using Friday.ERP.Server.Utilities.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;
using NLog;
using Oracle.ManagedDataAccess.Client;
using QuestPDF;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Configure Nlog
var logger = LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(),
    "/nlog.config")).GetCurrentClassLogger();

try
{
    builder.Services.AddHealthChecks();

    // Get Configuration
    var configuration = builder.Configuration;

    OracleConfiguration.TnsAdmin = "wwwroot/wallet";
    OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;

    // Add services to the container
    builder.Services.ConfigureCors();
    builder.Services.ConfigureIisIntegration();
    builder.Services.ConfigureLoggerService();

    builder.Services.ConfigureAppSettings(configuration);
    builder.Services.ConfigureDatabaseContext(configuration);
    builder.Services.ConfigureRepositoryManager();
    builder.Services.ConfigureServiceManager();

    builder.Services.ConfigureAuthentication(configuration);
    builder.Services.ConfigureApiBehaviorOptions(logger);

    // Add Action Filters
    builder.Services.AddScoped<ValidationFilterAttribute>();
    builder.Services.AddScoped<GetCurrentUserGuidActionFilter>();

    builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

    builder.Services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger();

    builder.Services.AddHttpClient();

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    // Add SignalR
    builder.Services.AddSignalR();
    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });

    Settings.License = LicenseType.Community;
    FontManager.RegisterFont(File.OpenRead("wwwroot/mm3-multi-os.ttf"));

    var app = builder.Build();

    app.MapHealthChecks("/");

    app.MigrateDatabase();

    // Setup Global Error Handling
    app.ConfigureExceptionHandler(logger);

    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Friday ERP API"); });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsProduction())
        app.UseHsts();
    else
        app.UseDeveloperExceptionPage();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });

    app.UseCors("CorsPolicy");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<NotificationHub>("/notification-hub");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}

// Todo : Versioning
// Todo : Data Shaping
// Todo : Caching
// Todo : RateLimit