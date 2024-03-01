using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using my_books.Data.ViewModel;
using System.Net;

namespace my_books.Exceptions
{
    //Here we are using static class to add the custom exception middleware
    public static class ExceptionMiddlewareExtensions
    {
        //this keyword is used because it is a extension method
        public static void ConfigureBuildiInExceptions(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(appError =>
            {
                //Inside we are construct the response, and here it is the delegate that handles the request
                appError.Run(async context =>
                {
                    var logger = loggerFactory.CreateLogger("ConfigureBuildiInExceptions");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //unhandle exception
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var contextRequest = context.Features.Get<IHttpRequestFeature>();//getting the request

                    if (contextFeature != null)
                    {
                        var errorVmString = new ErrorVM()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Path = contextRequest.Path
                        }.ToString();

                        logger.LogError(errorVmString);
                        await context.Response.WriteAsync(errorVmString);
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomeExceptionsMiddleware>();
        }
    }
}
