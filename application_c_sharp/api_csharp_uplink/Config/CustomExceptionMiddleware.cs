using api_csharp_uplink.DirException;
using System.Net;

namespace api_csharp_uplink.Config
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "text/html";
            if (exception is BusNotFoundException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }
            else if (exception is BusAlreadyCreateException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Conflict;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return context.Response.WriteAsync(exception.Message ?? "" );
        }
    }

}
