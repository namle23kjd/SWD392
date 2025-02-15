using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IShelfRepository
    {
        Task<IEnumerable<Shelf>> GetAllAsync();
        Task<Shelf?> GetByIdAsync(int id);
        Task<Shelf?> GetByCodeAsync(string code);
        Task<bool> IsCodeUniqueAsync(string code);
        Task CreateAsync(Shelf shelf);
        Task UpdateAsync(Shelf shelf);
        Task SaveChangesAsync();
    }
}
