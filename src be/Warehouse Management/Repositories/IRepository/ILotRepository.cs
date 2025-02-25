using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface ILotRepository
    {
        Task<PagedList<Lot>> GetAllAsync(int page, int pageSize);
        Task<Lot?> GetByIdAsync(int id);
        Task CreateAsync(Lot lot);
        Task UpdateAsync(Lot lot);
        Task SaveChangesAsync();
    }
}
