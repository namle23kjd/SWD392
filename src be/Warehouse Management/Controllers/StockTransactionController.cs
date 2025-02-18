using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Warehouse_Management.Models.DTO.StockTransaction;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionService _stockTransactionService;

        public StockTransactionController(IStockTransactionService stockTransactionService)
        {
            stockTransactionService = _stockTransactionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterDTO filter)
        { 
            if (filter.StartDate.HasValue && filter.EndDate.HasValue && filter.StartDate > filter.EndDate)
            {
                ModelState.AddModelError("DateRange", "Start date must be before or equal to end date");
                return BadRequest(ModelState);
            }

            // Ensure valid pagination parameters
            filter.Page = filter.Page < 1 ? 1 : filter.Page;
            filter.PageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            var response = await _stockTransactionService.GetTransactionsAsync(filter);

            if (response.IsSuccess && response.Result != null)
            {
                var paginationMetadata = new
                {
                    totalCount = response.Result.GetType().GetProperty("TotalCount")?.GetValue(response.Result, null),
                    pageSize = response.Result.GetType().GetProperty("PageSize")?.GetValue(response.Result, null),
                    currentPage = response.Result.GetType().GetProperty("CurrentPage")?.GetValue(response.Result, null),
                    totalPages = response.Result.GetType().GetProperty("TotalPages")?.GetValue(response.Result, null)
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            }

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            
            var response = await _stockTransactionService.GetTransactionByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("lot/{lotId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsByLotId(int lotId)
        {
            var response = await _stockTransactionService.GetTransactionsByLotIdAsync(lotId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateStockTransactionDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createDto.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than 0");
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token" });
            }

            var response = await _stockTransactionService.CreateTransactionAsync(createDto, userId);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var transaction = response.Result as StockTransactionDTO;
                return CreatedAtAction(
                    nameof(GetTransactionById),
                    new { id = transaction?.TransactionId },
                    response
                );
            }

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
