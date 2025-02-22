using Microsoft.AspNetCore.Identity;
using System.Net;
using Warehouse_Management.Data;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WareHouseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(WareHouseDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found." }
                };
            }

            var result = await _userManager.DeleteAsync(user);
            return new ApiResponse
            {
                StatusCode = result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                IsSuccess = result.Succeeded,
                Result = result.Succeeded ? "User deleted successfully." : null,
                ErrorMessages = result.Succeeded ? new List<string>() : result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<ApiResponse> EditUserAsync(string userId, EditUserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found." }
                };
            }

            // Cập nhật các thông tin từ DTO
            user.Email = userDto.Email;
            user.UserName = userDto.Email;  // Nếu tên người dùng là email
            user.PhoneNumber = userDto.PhoneNumber;

            // Cập nhật trạng thái (Lockout status)
            if (!userDto.Status)
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);  // Giả sử khoá tài khoản khi `Status = false`
            }
            else
            {
                user.LockoutEnd = null; // Kích hoạt tài khoản
            }

            var result = await _userManager.UpdateAsync(user);
            return new ApiResponse
            {
                StatusCode = result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                IsSuccess = result.Succeeded,
                Result = result.Succeeded ? "User updated successfully." : null,
                ErrorMessages = result.Succeeded ? new List<string>() : result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<ApiResponse> ModifyUserRoleAsync(string userId, string[] newRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found." }
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, newRoles);

            return new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "User roles updated successfully."
            };
        }

        public async Task<bool> RegisterAsync(IdentityUser user, string password, string[] roles)
        {
            // Tạo user
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                // Nếu có lỗi, ném ngoại lệ để middleware xử lý
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Kiểm tra và thêm role
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Failed to create role {role}");
                    }
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to assign role {role} to user {user.UserName}");
                }
            }

            return true;



        }
    }
}
