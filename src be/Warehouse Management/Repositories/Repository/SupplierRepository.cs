using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly WareHouseDbContext _db;
        public SupplierRepository(WareHouseDbContext db)
        {
            _db= db;
        }
        public async Task CreateAsync(Supplier supplier)
        {
            await _db.Suppliers.AddAsync(supplier);
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await GetByIdAsync(id);
            if (supplier != null)       
                _db.Suppliers.Remove(supplier);            
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _db.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByEmailAsync(string email)
        {
            return await _db.Suppliers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _db.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _db.Suppliers.Update(supplier); 
        }
    }
}
