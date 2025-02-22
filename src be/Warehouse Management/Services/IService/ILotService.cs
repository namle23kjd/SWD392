using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Lot;

namespace Warehouse_Management.Services.IService
{
    public interface ILotService
    {
        Task<ApiResponse> GetAllLotsAsync(int page, int pageSize);
        Task<ApiResponse> GetLotByIdAsync(int id);
        Task<ApiResponse> CreateLotAsync(CreateLotDTO dto);
        Task<ApiResponse> UpdateLotQuantityAsync(int id, int quantityChange);
    }
}
