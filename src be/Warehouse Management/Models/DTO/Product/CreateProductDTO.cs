using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Product
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "SKU is required")]
        [MinLength(1, ErrorMessage = "SKU cannot be empty")]
        [StringLength(50, ErrorMessage = "SKU must be at most 50 characters")]
        public string SKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "Barcode is required")]
        [MinLength(1, ErrorMessage = "Barcode cannot be empty")]
        [StringLength(100, ErrorMessage = "Barcode must be at most 100 characters")]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required")]
        [MinLength(1, ErrorMessage = "Product name cannot be empty")]
        [StringLength(200, ErrorMessage = "Product name must be at most 200 characters")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MinLength(1, ErrorMessage = "Description cannot be empty")]
        [StringLength(500, ErrorMessage = "Description must be at most 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Base price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0")]
        public decimal BasePrice { get; set; }
    }
}
