using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.StockTransaction;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IStockTransactionRepository
    {
        Task<PagedList<StockTransaction>> GetTransactionsAsync(TransactionFilterDTO filter);
        Task<StockTransaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<StockTransaction>> GetTransactionsByLotIdAsync(int lotId);
        Task AddTransactionAsync(StockTransaction transaction);
        Task SaveChangesAsync();
    }
}
