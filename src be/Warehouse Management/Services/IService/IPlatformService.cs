using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Platform;

namespace Warehouse_Management.Services.IService
{
    public interface IPlatformService
    {
        Task<ApiResponse> GetPlatformByIdAsync(int id);
        Task<ApiResponse> GetAllPlatformsAsync();
        Task<ApiResponse> CreatePlatformAsync(CreatePlatformDTO platformDto);
        Task<ApiResponse> UpdatePlatformAsync(int id, UpdatePlatformDTO platformDto);
        Task<ApiResponse> DeletePlatformAsync(int id);
    }
}
