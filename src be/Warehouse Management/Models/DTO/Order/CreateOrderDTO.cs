using Warehouse_Management.Models.DTO.OrderItem;

namespace Warehouse_Management.Models.DTO.Order
{
    public class CreateOrderDTO
    {
        public string UserId { get; set; }
        public int PlatformId { get; set; }
        public string PlatformOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CreateOrderItemDTO> OrderItems { get; set; }
    }
}
