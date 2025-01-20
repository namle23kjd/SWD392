using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Warehouse_Management.Middlewares
{
    public interface IExceptionHandler
    {
        ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken);
    }
}
