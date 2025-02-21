using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.StockTransaction;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly WareHouseDbContext _db;
        public StockTransactionRepository(WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task AddTransactionAsync(StockTransaction transaction)
        {
            await _db.StockTransactions.AddAsync(transaction);
        }

        public async Task<StockTransaction> GetTransactionByIdAsync(int id)
        {
            return await _db.StockTransactions
            .Include(t => t.Product)
            .Include(t => t.Supplier)
            .Include(t => t.Lot)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        public async Task<PagedList<StockTransaction>> GetTransactionsAsync(TransactionFilterDTO filter)
        {
            var query = _db.StockTransactions
            .Include(t => t.Product)
            .Include(t => t.Supplier)
            .Include(t => t.Lot)
            .Include(t => t.User)
            .AsQueryable();

            // Apply filters
            if (filter.StartDate.HasValue)
                query = query.Where(t => t.TransactionDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                query = query.Where(t => t.TransactionDate <= filter.EndDate);

            if (filter.ProductId.HasValue)
                query = query.Where(t => t.ProductId == filter.ProductId);

            if (filter.LotId.HasValue)
                query = query.Where(t => t.LotId == filter.LotId);

            if (filter.Type.HasValue)
                query = query.Where(t => t.Type == filter.Type.ToString());

            return await PagedList<StockTransaction>.CreateAsync(
                query.OrderByDescending(t => t.TransactionDate),
                filter.Page,
                filter.PageSize);
        }

        public async Task<IEnumerable<StockTransaction>> GetTransactionsByLotIdAsync(int lotId)
        {
            return await _db.StockTransactions
            .Include(t => t.Product)
            .Include(t => t.Supplier)
            .Include(t => t.User)
            .Where(t => t.LotId == lotId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
