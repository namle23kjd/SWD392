using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Supplier;

namespace Warehouse_Management.Services.IService
{
    public interface ISupplierService
    {
        Task<ApiResponse> GetAllSuppliersAsync();
        Task<ApiResponse> GetSupplierByIdAsync(int id);
        Task<ApiResponse> CreateSupplierAsync(CreateSupplierDTO dto);
        Task<ApiResponse> UpdateSupplierAsync(int id, UpdateSupplierDTO dto);
        Task<ApiResponse> HandleExceptionAsync(Exception ex);
    }
}
