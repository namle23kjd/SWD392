using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IPlatformRepository
    {
        Task<Platform> GetPlatformByIdAsync(int id);
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();
        Task AddPlatformAsync(Platform platform);
        Task UpdatePlatformAsync(Platform platform);
        Task DeletePlatformAsync(int id);
        Task SaveChangesAsync();
    }
}
