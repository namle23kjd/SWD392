using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Shelf
{
    public class CreateShelfDTO
    {
        [Required(ErrorMessage = "Shelf Code is required")]
        [StringLength(50, ErrorMessage = "Shelf Code cannot exceed 50 characters")]
        [RegularExpression(@"^[A-Z]{2}[0-9]+$", ErrorMessage = "Shelf Code must start with 2 uppercase letters followed by numbers (e.g., AB123)")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shelf Name is required")]
        [StringLength(100, ErrorMessage = "Shelf Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
    }
}
