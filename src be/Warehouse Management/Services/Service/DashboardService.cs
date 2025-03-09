using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public int GetTotalOrders()
        {
            return _dashboardRepository.GetTotalOrders();
        }

        public int GetTotalUsers()
        {
            return _dashboardRepository.GetTotalUsers();
        }

        public int GetTotalProducts()
        {
            return _dashboardRepository.GetTotalProducts();
        }

        public int GetTotalQuantity()
        {
            return _dashboardRepository.GetTotalQuantity();
        }
    }
}

