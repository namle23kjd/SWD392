namespace Warehouse_Management.Models.DTO
{
    public class ConfirmResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
