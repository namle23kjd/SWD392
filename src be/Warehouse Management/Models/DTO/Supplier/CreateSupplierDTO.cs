using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Supplier
{
    public class CreateSupplierDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}
