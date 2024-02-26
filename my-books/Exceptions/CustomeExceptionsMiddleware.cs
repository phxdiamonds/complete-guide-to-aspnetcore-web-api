using my_books.Data.ViewModel;
using System.Net;

namespace my_books.Exceptions
{
    public class CustomeExceptionsMiddleware
    {
        private readonly RequestDelegate _next;  //inject it to the constructor

        public CustomeExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex); // Create a new method
            }
        }

        private  Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            //construct the response object
            var response = new ErrorVM()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error from the custom Middlewware",
                Path = "Path-goes-here"
            };

            return httpContext.Response.WriteAsync(response.ToString());

        }
        
    }
}
