namespace Warehouse_Management.Models.DTO.OrderItem
{
    public class UpdateOrderItemDTO
    {
        public int OrderItemId { get; set; }  // Để cập nhật các item trong đơn hàng
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
