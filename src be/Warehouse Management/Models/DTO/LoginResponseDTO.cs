using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO
{
    public class LoginResponseDTO
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
