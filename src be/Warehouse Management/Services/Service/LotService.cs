using AutoMapper;
using System.ComponentModel.Design;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class LotService : ILotService
    {
        private readonly ILotRepository _lotRepository;
        private readonly IProductService _productService;
        private readonly IShelfService _shelfService;
        private readonly IMapper _mapper;
        private readonly ILogger<LotService> _logger;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
        public LotService(
            ILotRepository lotRepository,
            IProductService productService,
            IShelfService shelfService,
            IMapper mapper,
            ILogger<LotService> logger,
            IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _lotRepository = lotRepository;
            _productService = productService;
            _shelfService = shelfService;
            _mapper = mapper;
            _logger = logger;
            _exceptionHandlers = exceptionHandlers;
        }
        public async Task<ApiResponse> CreateLotAsync(CreateLotDTO dto)
        {
            try
            {
                var lot = _mapper.Map<Lot>(dto);
                lot.CreateAt = DateTime.UtcNow;

                await _lotRepository.CreateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = _mapper.Map<LotDTO>(lot)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllLotsAsync()
        {
            try
            {
                var lots = await _lotRepository.GetAllAsync();
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<IEnumerable<LotDTO>>(lots)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetLotByIdAsync(int id)
        {
            try
            {
                var lot = await _lotRepository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Không tìm thấy lô hàng với ID {id}.");

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<LotDTO>(lot)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> UpdateLotQuantityAsync(int id, int quantityChange)
        {
            try
            {
                var lot = await _lotRepository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Không tìm thấy lô hàng với ID {id}.");

                lot.Quantity += quantityChange;
                await _lotRepository.UpdateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<LotDTO>(lot)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        private async Task<ApiResponse> HandleExceptionAsync(Exception ex)
        {
            _logger.LogError(ex, "Lỗi xảy ra trong LotService");

            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = { "Đã xảy ra lỗi không mong muốn." }
            };
        }
    }
}
