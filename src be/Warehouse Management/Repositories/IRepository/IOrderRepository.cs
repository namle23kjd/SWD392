using Warehouse_Management.Models.Domain;

namespace Warehouse_Management.Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task SaveChangesAsync();
    }
}
