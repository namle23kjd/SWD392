using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
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
