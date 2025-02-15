namespace Warehouse_Management.Models.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool Status { get; set; }
    }
}
