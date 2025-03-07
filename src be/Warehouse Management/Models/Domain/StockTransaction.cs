using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.Domain
{
    public class StockTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        [ForeignKey("Lot")]
        public int LotId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public int Quantity { get; set; }
        public string? Type { get; set; }

        public DateTime TransactionDate { get; set; }

        public ApplicationUser? User { get; set; }

        public Product? Product { get; set; }
        public Supplier? Supplier { get; set; }
        public Lot? Lot { get; set; }
    }
}
