using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(ApplicationUser user,string password, string[] roles);
    }
}
