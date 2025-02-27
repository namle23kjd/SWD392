using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class LotRepository : ILotRepository
    {
        private readonly WareHouseDbContext _db;
        private readonly ILogger<LotRepository> _logger;
        public LotRepository(WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Lot lot)
        {
            await _db.Lots.AddAsync(lot);
        }

        public async Task<PagedList<Lot>> GetAllAsync(int page, int pageSize)
        {
            var query = _db.Lots
                .Include(l => l.Product)
                .Include(l => l.Shelf)
                .Include(l => l.User)
                .AsQueryable();

            return await PagedList<Lot>.CreateAsync(query, page, pageSize);
        }


        public async Task<Lot?> GetByIdAsync(int id)
        {
            return await  _db.Lots
            .Include(l => l.Product)
            .Include(l => l.Shelf)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.LotId == id);
        }

        public async Task<Lot?> GetByProductIdAsync(int productId)
        {
           var lots = await _db.Lots
                .Include(l => l.Product)
                .Include(l => l.Shelf)// Đảm bảo Load Product
                .Include(l => l.User) // Load User nếu cần
                .Where(l => l.ProductId == productId && l.Quantity > 0)
                .Where(l => l.ProductId == productId && l.Quantity > 0)
                .OrderBy(l => l.ManufactureDate)
                .ToListAsync();

            if (!lots.Any())
            {
                _logger.LogWarning($"No lot found for Product ID {productId}");
                return null;
            }

            var availableLot = lots.FirstOrDefault(l => l.Quantity > 0);
            if (availableLot == null)
            {
                _logger.LogWarning($"No available stock for Product ID {productId}");
            }

            return availableLot;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lot lot)
        {
            _db.Lots.Update(lot);
        }
    }
}
