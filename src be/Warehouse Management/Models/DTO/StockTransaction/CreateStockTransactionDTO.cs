using Warehouse_Management.Helpers.Enum;

namespace Warehouse_Management.Models.DTO.StockTransaction
{
    public class CreateStockTransactionDTO
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int LotId { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
    }
}
