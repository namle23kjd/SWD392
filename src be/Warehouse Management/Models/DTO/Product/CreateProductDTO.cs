namespace Warehouse_Management.Models.DTO.Product
{
    public class CreateProductDTO
    {
        public string? SKU { get; set; }

        public string? Barcode { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
