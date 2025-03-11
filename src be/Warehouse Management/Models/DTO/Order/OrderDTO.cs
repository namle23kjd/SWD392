using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.OrderItem;

namespace Warehouse_Management.Models.DTO.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int PlatformId { get; set; }
        public string? PlatformOrderId { get; set; }
        public bool OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderItemDTO>? OrderItems { get; set; }
    }
}
