using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? SKU { get; set; }

        public string? Barcode { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal? BasePrice { get; set; }

        public DateTime? CreatedAt { get; set; }  = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        public ICollection<StockTransaction>? StockTransactions { get; set; }
        public ICollection<Lot>? Lots { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
