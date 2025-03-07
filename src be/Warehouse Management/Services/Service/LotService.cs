using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Models.DTO.Order;
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
        private readonly IProductRepository _productRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public LotService(
            ILotRepository lotRepository,
            IProductService productService,
            IShelfService shelfService,
            IMapper mapper,
            ILogger<LotService> logger,
            IEnumerable<IExceptionHandler> exceptionHandlers,
            IProductRepository productRepository,
            IShelfRepository shelfRepository, 
            UserManager<IdentityUser> userManager)
        {
            _lotRepository = lotRepository;
            _productService = productService;
            _shelfService = shelfService;
            _mapper = mapper;
            _logger = logger;
            _exceptionHandlers = exceptionHandlers;
            _productRepository = productRepository;
            _shelfRepository = shelfRepository;
            _userManager = userManager;
        }
        public async Task<ApiResponse> CreateLotAsync(CreateLotDTO dto, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { "User not found" }
                    };
                }

                var productExists = await _productRepository.GetProductByIdAsync(dto.ProductId);
                if (productExists == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Product with ID {dto.ProductId} doesn't exist." }
                    };
                }

                // Kiểm tra kệ hàng có tồn tại không
                var shelfExists = await _shelfRepository.GetByIdAsync(dto.ShelfId);
                if (shelfExists == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Shelf with ID {dto.ShelfId} doesn't exist." }
                    };
                }
                var lot = _mapper.Map<Lot>(dto);
                lot.CreateAt = DateTime.UtcNow;
                lot.UpdateAt = DateTime.UtcNow;
                lot.UserId = userId;
                await _lotRepository.CreateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                var lotResponse = _mapper.Map<LotDTO>(lot);
                lotResponse.UserName = user.UserName;

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = lotResponse
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllLotsAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var pagedLots = await _lotRepository.GetAllAsync(page, pageSize);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        TotalCount = pagedLots.TotalCount,
                        PageSize = pagedLots.PageSize,
                        CurrentPage = pagedLots.CurrentPage,
                        TotalPages = pagedLots.TotalPages,
                        Lots = _mapper.Map<IEnumerable<LotDTO>>(pagedLots)
                    }
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
                var lot = await _lotRepository.GetByIdAsync(id);

                if (lot == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { $"Lot with ID {id} not found." }
                    };
                }

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

        public async Task<ApiResponse> UpdateLotAsync(int id, UpdateLotDTO dto)
        {
            try
            {
                var lot = await _lotRepository.GetByIdAsync(id);
                
                if (lot == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { $"Lot with ID {id} not found." }
                    };
                }
                var productExists = await _productRepository.GetProductByIdAsync(dto.ProductId);
                if (productExists == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Product with ID {dto.ProductId} doesn't exist." }
                    };
                }

                // Kiểm tra kệ hàng có tồn tại không
                var shelfExists = await _shelfRepository.GetByIdAsync(dto.ShelfId);
                if (shelfExists == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Shelf with ID {dto.ShelfId} doesn't exist." }
                    };
                }
                if (lot.Quantity + dto.Quantity < 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "The lot quantity cannot be less than 0." }
                    };
                }

                lot.Quantity = dto.Quantity;
                lot.ProductId = dto.ProductId;
                lot.ShelfId = dto.ShelfId;
                lot.LotCode = dto.LotCode;
                lot.UpdateAt = DateTime.Now;
                await _lotRepository.UpdateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Updated lot successfully",
                        LotID = lot.LotId,
                        productId = lot.ProductId,
                        shelfId = lot.ShelfId,
                        lotCode = lot.LotCode,
                        Quantity = lot.Quantity
                    }
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
                var lot = await _lotRepository.GetByIdAsync(id);

                if (lot == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { $"Lot with ID {id} not found." }
                    };
                }
                if (lot.Quantity + quantityChange < 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "The lot quantity cannot be less than 0." }
                    };
                }

                lot.Quantity = quantityChange;
                await _lotRepository.UpdateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Updated quantity successfully",
                        LotID = lot.LotId,
                        Quantity = lot.Quantity
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        private async Task<ApiResponse> HandleExceptionAsync(Exception ex)
        {
            _logger.LogError(ex, "Error in LotService");

            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = { "Unexpected error." }
            };
        }
    }
}
