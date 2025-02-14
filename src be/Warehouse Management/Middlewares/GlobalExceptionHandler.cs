using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Warehouse_Management.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public GlobalExceptionHandler(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                using var scope = _scopeFactory.CreateScope();
                var handlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IExceptionHandler>>();

                foreach (var handler in handlers)
                {
                    if (await handler.TryHandleAsync(context, ex, CancellationToken.None))
                    {
                        return;
                    }
                }

                // Nếu không có handler nào xử lý lỗi, trả lỗi mặc định
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}
