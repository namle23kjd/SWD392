using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.DTO.Product
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public int? LotId { get; set; }
        public string? SKU { get; set; }

        public string? Barcode { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal? BasePrice { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserName { get; set; }
    }
}