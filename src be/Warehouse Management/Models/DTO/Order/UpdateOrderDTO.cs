using Warehouse_Management.Models.DTO.OrderItem;

namespace Warehouse_Management.Models.DTO.Order
{
    public class UpdateOrderDTO
    {
        public bool OrderStatus { get; set; }
        public List<UpdateOrderItemDTO> OrderItems { get; set; } = new();
    }
}
