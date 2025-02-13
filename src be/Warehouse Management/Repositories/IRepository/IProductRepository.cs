using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetALlProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);

        Task<IEnumerable<Product>> SearchProductAsync(string? sku, string? barcode, string? name);

        Task AddProductAsync(Product product);
        void UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task SaveChangesAsync();
    }
}
