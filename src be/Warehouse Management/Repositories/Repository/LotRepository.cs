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
