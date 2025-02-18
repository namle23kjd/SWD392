using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly WareHouseDbContext _db;
        public ShelfRepository(WareHouseDbContext db)
        {
            _db = db;   
        }
        public async Task CreateAsync(Shelf shelf)
        {
            await _db.Shelves.AddAsync(shelf);
        }

        public async Task<IEnumerable<Shelf>> GetAllAsync()
        {
            return await _db.Shelves.Include(x => x.User).ToListAsync();
        }

        public async Task<Shelf?> GetByCodeAsync(string code)
        {
            return await _db.Shelves.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Shelf?> GetByIdAsync(int id)
        {
            return await _db.Shelves.Include(x => x.User).FirstOrDefaultAsync(x => x.ShelfId == id);
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            return !await _db.Shelves.AnyAsync(x => x.Code == code);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shelf shelf)
        {
            _db.Shelves.Update(shelf);
        }
    }
}
