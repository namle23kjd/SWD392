namespace Warehouse_Management.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
        public bool Status { get; set; }
    }
}
