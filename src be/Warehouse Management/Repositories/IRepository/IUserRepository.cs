using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(IdentityUser user,string password, string[] roles);
    }
}
