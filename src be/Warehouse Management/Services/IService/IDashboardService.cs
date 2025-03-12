namespace Warehouse_Management.Services.IService
{
    public interface IDashboardService
    {
        int GetTotalProducts();
        int GetTotalOrders();
        int GetTotalUsers();
        int GetTotalQuantity();
    }
}
