using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly WareHouseDbContext _db;
        public ProductRepository( WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task AddProductAsync(Product product) 
            => await _db.Products.AddAsync(product);


        public async Task DeleteProductAsync(Product product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedList<Product>> GetALlProductsAsync(int pageNumber, int pageSize)
        {
            var query = _db.Products.Include(p => p.User).AsQueryable();
            return await PagedList<Product>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        => await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id);

        public async Task SaveChangesAsync()
        => await _db.SaveChangesAsync();

        public async Task<IEnumerable<Product>> SearchProductAsync(string? sku, string? barcode, string? name)
        {
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(sku)) query = query.Where(p => p.SKU.Contains(sku));
            if (!string.IsNullOrEmpty(barcode)) query = query.Where(p => p.Barcode.Contains(barcode));
            if (!string.IsNullOrEmpty(name)) query = query.Where(p => p.ProductName.Contains(name));
            return await query.ToListAsync();
        }

        public void UpdateProductAsync(Product product)
        => _db.Products.Update(product);
    }
}
