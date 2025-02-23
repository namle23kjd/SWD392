using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Lot
{
    public class CreateLotDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ShelfId { get; set; }

        [Required]
        [StringLength(50)]
        public string LotCode { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        public DateTime ManufactureDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
    