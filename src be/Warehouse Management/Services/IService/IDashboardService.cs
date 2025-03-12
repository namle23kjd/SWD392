
namespace Warehouse_Management.Services.IService {
    public interface IDashboardService
    {
        int GetTotalProducts();
        int GetTotalOrders();
        int GetTotalUsers();
        int GetTotalQuantity();

        IEnumerable<object> GetOrdersByPlatform();
        IEnumerable<object> GetAllSuppliers();
        object GetTransactionTypeSummary();
        IEnumerable<object> GetLowStockProducts();
    }
}



