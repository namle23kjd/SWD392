namespace Warehouse_Management.Models.DTO.Product
{
    public class CreateProductFromTransaction
    {
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int SupplierId { get; set; }
        public int ShelfId { get; set; }
        public int Quantity { get; set; }

        public DateTime ManufactureDate { get; set; } 
        public DateTime ExpiryDate { get; set; } 

        public string UserId { get; set; }

    }
}
