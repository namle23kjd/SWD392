using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface ILotRepository
    {
        Task<(IEnumerable<Lot> lots, int totalCount)> GetAllAsync(int page, int pageSize);
        Task<Lot?> GetByIdAsync(int id);
        Task CreateAsync(Lot lot);
        Task UpdateAsync(Lot lot);
        Task SaveChangesAsync();
    }
}
