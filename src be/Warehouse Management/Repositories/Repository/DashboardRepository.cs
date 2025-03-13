using Warehouse_Management.Data;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Models.DTO.Product;
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

        public IEnumerable<object> GetOrdersByPlatform()
        {
            return _context.Orders
                           .GroupBy(o => o.Platform.Name)
                           .Select(g => new
                           {
                               platform = g.Key,
                               orders = g.Count()
                           })
                           .ToList();
        }
        public IEnumerable<object> GetAllSuppliers()
        {
            return _context.StockTransactions
                           .Where(st => st.Type == "IMPORT")  // Chỉ lấy các giao dịch nhập hàng
                           .GroupBy(st => st.Supplier.Name)  // Nhóm theo tên nhà cung cấp
                           .Select(g => new
                           {
                               supplierName = g.Key,  // Tên nhà cung cấp
                               totalImport = g.Count()  // Tổng nhập hàng của nhà cung cấp
                           })
                           .OrderByDescending(s => s.totalImport)  // Sắp xếp theo tổng nhập hàng (nếu cần)
                           .ToList();  // Lấy tất cả nhà cung cấp
        }


        public object GetTransactionTypeSummary()
        {
            var importQuantity = _context.StockTransactions
                                          .Where(st => st.Type == "Import")
                                          .Sum(st => st.Quantity);
            var exportQuantity = _context.StockTransactions
                                           .Where(st => st.Type == "Export")
                                           .Sum(st => st.Quantity);

            return new
            {
                import = importQuantity,
                export = exportQuantity
            };
        }
        public IEnumerable<object> GetLowStockProducts()
        {
            var warningDate = DateTime.UtcNow.AddMonths(3); // Tính toán ngày cảnh báo (CreateAt + 3 tháng)

            return _context.Lots
                           .Where(l => l.ExpiryDate <= warningDate) // Kiểm tra ngày hết hạn trong vòng 3 tháng
                           .GroupBy(l => l.LotId) // Nhóm theo LotId
                           .Select(g => new
                           {
                               lot = $"lot{g.Key}", // Tạo key "lot1", "lot2", ...
                               products = g.Count() // Số lượng sản phẩm trong Lot có ngày hết hạn gần
                           })
                           .ToList();
        }

    }
}
