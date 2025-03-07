namespace Warehouse_Management.Models.DTO.OrderItem
{
    public class UpdateOrderItemDTO
    {
        public int OrderItemId { get; set; }  // Nếu = 0 => tạo mới, nếu có ID => cập nhật
        public int Quantity { get; set; }
    }
}
