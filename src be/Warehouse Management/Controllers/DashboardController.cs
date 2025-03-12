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
    }
}
