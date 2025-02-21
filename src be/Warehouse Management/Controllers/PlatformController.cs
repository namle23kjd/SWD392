using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Management.Models.DTO.Platform;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            var response = await _platformService.GetPlatformByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var response = await _platformService.GetAllPlatformsAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform([FromBody] CreatePlatformDTO platformDto)
        {
            var response = await _platformService.CreatePlatformAsync(platformDto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatform(int id, [FromBody] UpdatePlatformDTO platformDto)
        {
            var response = await _platformService.UpdatePlatformAsync(id, platformDto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(int id)
        {
            var response = await _platformService.DeletePlatformAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
