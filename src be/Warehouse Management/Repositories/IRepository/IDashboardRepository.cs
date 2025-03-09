namespace Warehouse_Management.Repositories.IRepository
{
    public interface IDashboardRepository
    {
        int GetTotalProducts();
        int GetTotalOrders();
        int GetTotalUsers();
        int GetTotalQuantity();

    }
}
