using Microsoft.AspNetCore.Mvc;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/dashboard
        [HttpGet]
        public IActionResult GetDashboardData()
        {
            var totalOrders = _dashboardService.GetTotalOrders();
            var totalUsers = _dashboardService.GetTotalUsers();
            var totalProducts = _dashboardService.GetTotalProducts();
            var totalQuantity = _dashboardService.GetTotalQuantity();

            var dashboardData = new
            {
                TotalExports = totalOrders,
                TotalUsers = totalUsers,
                TotalImports = totalProducts,
                TotalQuantity = totalQuantity
            };

            return Ok(dashboardData);
        }

        //GET: 
        [HttpGet("orders-by-platform")]
        public IActionResult GetOrdersByPlatform()
        {
            var platformData = _dashboardService.GetOrdersByPlatform();
            return Ok(platformData);
        }


        // Phương thức lấy tất cả các nhà cung cấp
        [HttpGet("all-suppliers")]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _dashboardService.GetAllSuppliers();  // Gọi từ service
            return Ok(suppliers);  // Trả về kết quả cho client
        }



        //[HttpGet("transaction-type-summary")]
        //public IActionResult GetTransactionTypeSummary()
        //{
        //    var transactionData = _dashboardService.GetTransactionTypeSummary();
        //    return Ok(transactionData);
        //}


        [HttpGet("low-stock-products")]
        public IActionResult GetLowStockProducts()
        {
            var lowStockProducts = _dashboardService.GetLowStockProducts();
            return Ok(lowStockProducts);
        }


        [HttpGet("top-products")]
        public IActionResult GetTopOrderProducts()
        {
            var topOrderProducts = _dashboardService.GetTopOrderProducts();
            return Ok(topOrderProducts);
        }



    }

}
