using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Management.Models.DTO.Supplier;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        /// <summary>
        /// Get all suppliers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _supplierService.GetAllSuppliersAsync(pageNumber,pageSize);
            return StatusCode((int)response.StatusCode, response);
        }


        /// <summary>
        /// Get supplier by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var response = await _supplierService.GetSupplierByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Create Supplier
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSupplier ([FromBody] CreateSupplierDTO dto)
        {
            var response = await _supplierService.CreateSupplierAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }


        /// <summary>
        /// Update Supplier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO dto)
        {
            var response = await _supplierService.UpdateSupplierAsync(id, dto);
            return StatusCode((int)response.StatusCode, response);
        }



        // <summary>
        /// Delete a supplier by ID
        /// </summary>
        /// <param name="id">The ID of the supplier to be deleted</param>
        /// /// <returns>Returns an IActionResult with the API response</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var response = await _supplierService.DeleteSupplierAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
