using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Product;

namespace Warehouse_Management.Services.IService
{
    public interface IUserService
    {
        Task<ApiResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<LoginResponseDTO> GenerateJwtToken(IdentityUser applicationUser);

        Task<ApiResponse> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<ApiResponse> ResetPasswordAsync(string email);
        Task<ApiResponse> ConfirmResetPasswordAsync(string email, string token, string newPassword);
        Task<ApiResponse> EditUserAsync(string userId, EditUserDTO userDto);
        Task<ApiResponse> DeleteUserAsync(string userId);
        Task<ApiResponse> ModifyUserRoleAsync(string userId, string[] newRoles);
        Task<ApiResponse> GetAllUserAsync();
    }
}
