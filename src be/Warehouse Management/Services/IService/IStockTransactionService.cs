using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.StockTransaction;

namespace Warehouse_Management.Services.IService
{
    public interface IStockTransactionService
    {
        Task<ApiResponse> CreateTransactionAsync(CreateStockTransactionDTO dto, string userId);
        Task<ApiResponse> GetTransactionByIdAsync(int id);
        Task<ApiResponse> GetTransactionsAsync(TransactionFilterDTO filter);
        Task<ApiResponse> GetTransactionsByLotIdAsync(int lotId);
    }
}
