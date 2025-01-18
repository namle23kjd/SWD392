using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models
{
    public class Lot
    {
        [Key]
        public int LotId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Shelf")]
        public int ShelfId { get; set; }
        public string? LotCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
        public Product? Product { get; set; }
        public Shelf? Shelf { get; set; }
        public ICollection<StockTransaction>? StockTransactions { get; set; }
    }
}
