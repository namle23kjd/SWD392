using Warehouse_Management.Models.DTO.OrderItem;

namespace Warehouse_Management.Models.DTO.Order
{
    public class UpdateOrderDTO
    {
        public int PlatformId { get; set; }
        public string PlatformOrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<UpdateOrderItemDTO> OrderItems { get; set; }
    }
}
