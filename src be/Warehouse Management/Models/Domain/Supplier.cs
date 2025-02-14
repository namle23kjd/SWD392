using System.ComponentModel.DataAnnotations;

namespace Warehouse_Management.Models.Domain
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreateAt { get; set; }
        public ICollection<StockTransaction>? StockTransactions { get; set; }
    }
}
