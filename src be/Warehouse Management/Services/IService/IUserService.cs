using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;

namespace Warehouse_Management.Services.IService
{
    public interface IUserService
    {
        Task<ApiResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<LoginResponseDTO> GenerateJwtToken(IdentityUser applicationUser);

        Task<ApiResponse> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<ApiResponse> ResetPasswordAsync(string email);
        Task<ApiResponse> ConfirmResetPasswordAsync(string email, string token, string newPassword);
    }
}
