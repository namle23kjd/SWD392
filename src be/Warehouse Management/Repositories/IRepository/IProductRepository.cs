using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetALlProductsAsync(int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(int id);

        Task<IEnumerable<Product>> SearchProductAsync(string? sku, string? barcode, string? name);

        Task AddProductAsync(Product product);
        void UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<Product?> GetProductBySKUAsync(string sku);
        Task SaveChangesAsync();
    }
}
