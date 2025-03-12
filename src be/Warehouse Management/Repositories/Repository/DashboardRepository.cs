using Warehouse_Management.Data;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly WareHouseDbContext _context;

        public DashboardRepository(WareHouseDbContext context)
        {
            _context = context;
        }

        public int GetTotalOrders()
        {
            return _context.Orders.Count();
        }

        public int GetTotalProducts()
        {
            return _context.Products.Count();
        }

        public int GetTotalUsers()
        {
            return _context.Users.Count();
        }

        public int GetTotalQuantity()
        {
            return _context.Lots.Sum(l => l.Quantity);
        }
    }
}
