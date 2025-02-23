using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.DTO.Lot
{
    public class UpdateLotQuantityDTO
    {

        [Required]
        public int Quantity { get; set; }
    }
}
