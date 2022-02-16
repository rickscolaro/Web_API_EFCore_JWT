


using System.Net;
using APICatalogo_Fundamentos.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace APICatalogo_Fundamentos.Extensions {

    public static class ApiExceptionMiddlewareExtensions {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app) {

            app.UseExceptionHandler(appError => {

                appError.Run(async context => {
                    
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null) {

                        await context.Response.WriteAsync(new ErrorDetails() {
                            
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace

                        }.ToString());
                    }
                });
            });
        }
    }
}