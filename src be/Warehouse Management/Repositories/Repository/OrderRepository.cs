using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WareHouseDbContext _db;
        public OrderRepository(WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _db.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _db.Orders
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(o => o.OrderItems)
                    .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _db.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _db.Orders.Update(order);
        }
    }
}
