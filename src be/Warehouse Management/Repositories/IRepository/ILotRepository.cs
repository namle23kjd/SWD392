using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface ILotRepository
    {
        Task<IEnumerable<Lot>> GetAllAsync();
        Task<Lot?> GetByIdAsync(int id);
        Task CreateAsync(Lot lot);
        Task UpdateAsync(Lot lot);
        Task SaveChangesAsync();
    }
}
