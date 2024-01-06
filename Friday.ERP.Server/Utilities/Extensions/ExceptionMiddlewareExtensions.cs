using Friday.ERP.Core.Exceptions.BadRequest;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.Exceptions.Unauthorized;
using Friday.ERP.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using NLog;

namespace Friday.ERP.Server.Utilities.Extensions;

internal static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this WebApplication app, Logger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var refNo = Guid.NewGuid();
                    ErrorResponseDto<string> response = new()
                    {
                        ErrorReferenceNumber = refNo,
                        ErrorType = contextFeature.Error switch
                        {
                            NotFoundException => "Not Found",
                            BadRequestException => "Bad Request",
                            UnauthorizedException => "Unauthorized",
                            _ => "Unknown System Exception"
                        },
                        ErrorDetail = new List<string> { contextFeature.Error.Message }
                    };

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        UnauthorizedException => StatusCodes.Status401Unauthorized,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var logText =
                        $"ErrorReferenceNumber : {refNo}. Response : {JsonConvert.SerializeObject(response)}. Path => {contextFeature.Path}.";

                    switch (contextFeature.Error)
                    {
                        case NotFoundException or BadRequestException or UnauthorizedException:
                            logger.Info(logText);
                            break;

                        default:
                            logger.Error(logText);
                            break;
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        });
    }
}