using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Product;

namespace Warehouse_Management.Services.IService
{
    public interface IProductService
    {
        Task<ApiResponse> GetAllProductsAsync();
        Task<ApiResponse> GetProductByIdAsync(int id);
        Task<ApiResponse> SearchProductsAsync(string? sku, string? barcode, string? name);
        Task<ApiResponse> CreateProductAsync(CreateProductDTO productDto, string userId);
        Task<ApiResponse> UpdateProductAsync(int id, UpdateProductDTO productDto);
        Task<ApiResponse> DeleteProductAsync(int id);

        Task<ApiResponse> HandleExceptionAsync(Exception ex);
    }
}
