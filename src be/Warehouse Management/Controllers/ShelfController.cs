using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Management.Models.DTO.Shelf;
using Warehouse_Management.Services.Service;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelfController : ControllerBase
    {
        private readonly ShelfService _shelfService;

        public ShelfController(ShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShelves()
        {
            var response = await _shelfService.GetAllShelvesAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShelfById(int id)
        {
            var response = await _shelfService.GetShelfByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShelf([FromBody] CreateShelfDTO dto)
        {
            var response = await _shelfService.CreateShelfAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShelf(int id, [FromBody] CreateShelfDTO dto)
        {
            var response = await _shelfService.UpdateShelfAsync(id, dto);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
