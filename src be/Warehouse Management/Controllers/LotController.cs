using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Services.IService;
using Warehouse_Management.Services.Service;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly ILotService _lotService;

        public LotController(ILotService lotService)
        {
            _lotService = lotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLots([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _lotService.GetAllLotsAsync(pageNumber, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLotById(int id)
        {
            var response = await _lotService.GetLotByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateLot([FromBody] CreateLotDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "User not found" });
            var response = await _lotService.CreateLotAsync(dto, userId);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPut("{id:int}/quantity")]
        public async Task<IActionResult> UpdateLotQuantity(int id, [FromBody] UpdateLotQuantityDTO dto)
        {
            var response = await _lotService.UpdateLotQuantityAsync(id, dto.Quantity);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLot(int id, UpdateLotDTO dto)
        {
            var response = await _lotService.UpdateLotAsync(id, dto);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
