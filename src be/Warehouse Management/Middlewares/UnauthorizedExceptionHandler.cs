using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Warehouse_Management.Middlewares
{
    public class UnauthorizedExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is UnauthorizedAccessException)
            {
                var details = new ProblemDetails
                {
                    Detail = "You are not authorized to perform this action.",
                    Instance = "Authorization",
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Type = "https://httpstatuses.com/401"
                };

                var response = JsonSerializer.Serialize(details);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsync(response, cancellationToken);

                return true;
            }

            return false;
        }
    }

}
