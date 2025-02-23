using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Shelf;

namespace Warehouse_Management.Services.IService
{
    public interface IShelfService
    {
       Task<ApiResponse> GetAllShelvesAsync(int page, int pageSize);
        Task<ApiResponse> GetShelfByIdAsync(int id);
        Task<ApiResponse> CreateShelfAsync(CreateShelfDTO dto);
        Task<ApiResponse> UpdateShelfAsync(int id, CreateShelfDTO dto);
        Task<ApiResponse> DeleteShelfAsync(int id);
        Task<ApiResponse> HandleExceptionAsync (Exception ex);
    }
}
