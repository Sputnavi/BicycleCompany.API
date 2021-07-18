using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace BicycleCompany.BLL.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<ExceptionHandlerFeature>();
                    if (context != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(
                                new ErrorResponseModel(context.Response.StatusCode, "Internal Server Error.")));
                    }

                });
            });
        }
    }
}
