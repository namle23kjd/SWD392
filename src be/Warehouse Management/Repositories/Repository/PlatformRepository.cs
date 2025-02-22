using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly WareHouseDbContext _db;

        public PlatformRepository(WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task AddPlatformAsync(Platform platform)
        {
            await _db.Platforms.AddAsync(platform);
        }

        public async Task DeletePlatformAsync(int id)
        {
            var platform = await GetPlatformByIdAsync(id);
            if (platform != null) _db.Platforms.Remove(platform); 
        }

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
        {
            return await _db.Platforms.ToListAsync();
        }

        public async Task<Platform> GetPlatformByIdAsync(int id)
        {
            return await _db.Platforms.FirstOrDefaultAsync(x => x.PlatformId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePlatformAsync(Platform platform)
        {
            _db.Platforms.Update(platform);
        }
    }
}
