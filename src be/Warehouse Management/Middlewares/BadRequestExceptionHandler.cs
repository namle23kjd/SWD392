using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Warehouse_Management.Middlewares
{
    public class BadRequestExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is BadHttpRequestException)
            {
                var details = new ProblemDetails
                {
                    Detail = $"An error occurred: {exception.Message}",
                    Instance = "Request",
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Type = "https://httpstatuses.com/400"
                };

                var response = JsonSerializer.Serialize(details);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsync(response, cancellationToken);

                return true; 
            }

            return false;
        }
    }
}
