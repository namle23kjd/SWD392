using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return StatusCode((int)products.StatusCode, products);
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? sku, [FromQuery] string? barcode, [FromQuery] string? name)
        {
            var response = await _productService.SearchProductsAsync(sku, barcode, name);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "User not found" });

            var response = await _productService.CreateProductAsync(productDto, userId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            var response = await _productService.UpdateProductAsync(id, productDto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent(); // ✅ Trả về đúng phản hồi `204 No Content` mà không có body
            }

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
