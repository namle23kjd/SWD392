using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Shelf
{
    public class UpdateShelfDTO
    {
        [StringLength(100, ErrorMessage = "Shelf Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int? Capacity { get; set; }
    }
}
