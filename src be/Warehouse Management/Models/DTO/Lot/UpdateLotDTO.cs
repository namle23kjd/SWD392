using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Lot
{
    public class UpdateLotDTO
    {
        [Required]
        public int Quantity { get; set; }
    }
}
