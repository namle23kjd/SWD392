using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Warehouse_Management.Helpers.Enum;

namespace Warehouse_Management.Models.DTO.StockTransaction
{
    public class StockTransactionDTO
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int LotId { get; set; }
        public string LotCode { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}