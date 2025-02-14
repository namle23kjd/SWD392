namespace Warehouse_Management.Models.DTO.OrderItem
{
    public class CreateOrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
