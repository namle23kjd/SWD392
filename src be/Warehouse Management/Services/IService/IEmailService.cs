using Warehouse_Management.Helpers;

namespace Warehouse_Management.Services.IService
{
    public interface IEmailService
    {
        Task<ApiResponse> SendEmailAsync(string toEmail, string subject, string body);
    }
}
