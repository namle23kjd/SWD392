using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy danh sách tất cả đơn hàng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _orderService.GetAllOrdersAsync(pageNumber,pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Lấy chi tiết đơn hàng theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Tạo mới đơn hàng
        /// </summary>z
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _orderService.CreateOrderAsync(orderDto);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng theo ID
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _orderService.UpdateOrderAsync(id, orderDto);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Xóa đơn hàng theo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
