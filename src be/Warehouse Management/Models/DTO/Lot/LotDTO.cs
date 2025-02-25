namespace Warehouse_Management.Models.DTO.Lot
{
    public class LotDTO
    {
        public int LotId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int ShelfId { get; set; }
        public string? ShelfName { get; set; }
        public string LotCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? UserName { get; set; }

    }
}
