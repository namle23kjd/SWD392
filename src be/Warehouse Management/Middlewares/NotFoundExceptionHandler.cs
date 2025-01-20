
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Warehouse_Management.Middlewares
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(exception is KeyNotFoundException)
            {
                var details = new ProblemDetails
                {
                    Detail = $"An error occured : {exception.Message}",
                    Instance = "Request",
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Type = "https://httpstatuses.com/404"
                };
                var response = JsonSerializer.Serialize(details);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                await httpContext.Response.WriteAsync(response, cancellationToken);
                return true;
            }
            return false;
        }
    }
}
