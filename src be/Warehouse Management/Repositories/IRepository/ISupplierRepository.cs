using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface ISupplierRepository
    {
        Task<(IEnumerable<Supplier> suppliers, int totalCount)> GetAllAsync(int page, int pageSize);

        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> GetByEmailAsync(string email);
        Task CreateAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
