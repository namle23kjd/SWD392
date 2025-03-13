using Warehouse_Management.Models.DTO.Lot;
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
        public IEnumerable<object> GetOrdersByPlatform()
        {
            return _dashboardRepository.GetOrdersByPlatform();
        }
        public IEnumerable<object> GetAllSuppliers()
        {
            return _dashboardRepository.GetAllSuppliers();  // Gọi từ repository
        }

        public object GetTransactionTypeSummary()
        {
            return _dashboardRepository.GetTransactionTypeSummary();
        }
        public IEnumerable<object> GetLowStockProducts()
        {
            return _dashboardRepository.GetLowStockProducts();
        }

    }
}

