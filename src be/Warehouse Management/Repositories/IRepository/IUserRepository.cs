using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Product;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(IdentityUser user,string password, string[] roles);
        Task<ApiResponse> EditUserAsync(string userId, EditUserDTO userDto);
        Task<ApiResponse> DeleteUserAsync(string userId);
        Task<ApiResponse> ModifyUserRoleAsync(string userId, string[] newRoles);
    }
}
