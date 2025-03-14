
namespace Warehouse_Management.Repositories.IRepository
{
    public interface IDashboardRepository
    {
        int GetTotalProducts();
        int GetTotalOrders();
        int GetTotalUsers();
        int GetTotalQuantity();
        IEnumerable<object> GetOrdersByPlatform();
        IEnumerable<object> GetAllSuppliers();
        //object GetTransactionTypeSummary();
        IEnumerable<object> GetLowStockProducts();

        IEnumerable<object> GetTopOrderProducts();

    }
}
