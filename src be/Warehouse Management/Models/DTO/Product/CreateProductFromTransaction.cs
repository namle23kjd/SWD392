using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Product
{
    public class CreateProductFromTransaction
    {
        [Required(ErrorMessage = "SKU is required.")]
        [RegularExpression(@"^SKU\d{3}$", ErrorMessage = "SKU must be in the format 'SKUxxx', where 'xxx' are numbers.")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Barcode is required.")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Base price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0.")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Shelf ID is required.")]
        public int ShelfId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Manufacture date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Manufacture date is not valid.")]
        public DateTime ManufactureDate { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Expiry date is not valid.")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }
    }
}
