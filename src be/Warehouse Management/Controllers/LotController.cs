using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Services.IService;

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
        public async Task<IActionResult> GetAllLots()
        {
            var response = await _lotService.GetAllLotsAsync();
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
            var response = await _lotService.CreateLotAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPut("{id:int}/quantity")]
        public async Task<IActionResult> UpdateLotQuantity(int id, [FromBody] UpdateLotDTO dto)
        {
            var response = await _lotService.UpdateLotQuantityAsync(id, dto.Quantity);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
