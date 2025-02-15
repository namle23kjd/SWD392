using Warehouse_Management.Helpers.Enum;

namespace Warehouse_Management.Models.DTO.StockTransaction
{
    public class TransactionFilterDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProductId { get; set; }
        public int? LotId { get; set; }
        public TransactionType? Type { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
