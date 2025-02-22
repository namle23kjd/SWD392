namespace Warehouse_Management.Models.DTO.Product
{
    public class EditUserDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string[] Roles { get; set; }
        public bool Status { get; set; }
    }
}
