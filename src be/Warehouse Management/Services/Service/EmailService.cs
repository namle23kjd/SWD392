using System.Net.Mail;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ApiResponse> SendEmailAsync(string toEmail, string subject, string body)
        {
            var response = new ApiResponse();

            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");

                using (var client = new SmtpClient(emailSettings["Host"], int.Parse(emailSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(emailSettings["Email"], emailSettings["Password"]);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(emailSettings["Email"], emailSettings["DisplayName"]),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);

                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                    response.Result = "Email đã được gửi thành công.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Lỗi khi gửi email: " + ex.Message);
            }

            return response;
        }
    }
}
