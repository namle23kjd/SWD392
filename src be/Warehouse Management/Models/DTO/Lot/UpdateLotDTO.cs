using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.DTO.Lot
{
    public class UpdateLotDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ShelfId { get; set; }

        [Required]
        public string? LotCode { get; set; }
        
        [Required]
        public int Quantity { get; set; }
    }
}
