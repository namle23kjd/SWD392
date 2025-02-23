using AutoMapper;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Shelf;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Repositories.Repository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository _shelfRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShelfService> _logger;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

        public ShelfService(IShelfRepository shelfRepository, IMapper mapper, ILogger<ShelfService> logger, IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _exceptionHandlers = exceptionHandlers;
            _logger = logger;
            _mapper = mapper;
            _shelfRepository = shelfRepository;
        }
        public  async Task<ApiResponse> CreateShelfAsync(CreateShelfDTO dto)
        {
            try
            { 
                if (!await _shelfRepository.IsCodeUniqueAsync(dto.Code))
                    throw new Exception($"Shelf code {dto.Code} is already taken");
                var shelf = _mapper.Map<Shelf>(dto);
                shelf.CreatedAt = DateTime.UtcNow;
                shelf.UpdatedAt = DateTime.UtcNow;

                await _shelfRepository.CreateAsync(shelf);
                await _shelfRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = _mapper.Map<ShelfDTO>(shelf)
                };
            }
            catch(Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllShelvesAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var (shelves, totalCount) = await _shelfRepository.GetAllAsync(page, pageSize);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        TotalCount = totalCount,
                        Shelves = _mapper.Map<IEnumerable<ShelfDTO>>(shelves)
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetShelfByIdAsync(int id)
        {
            try
            {
                var shelf = await _shelfRepository.GetByIdAsync(id);
                if (shelf == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { $"Shelf with ID {id} not found" }
                    };
                }

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<ShelfDTO>(shelf)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> HandleExceptionAsync(Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = { "Internal Server Error" }
            };
            
        }

        public async Task<ApiResponse> UpdateShelfAsync(int id, CreateShelfDTO dto)
        {
            try
            {
                var shelf = await _shelfRepository.GetByIdAsync(id);
                if (shelf == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { $"Shelf with ID {id} not found" }
                    };
                }

                _mapper.Map(dto, shelf);
                shelf.UpdatedAt = DateTime.UtcNow;

                await _shelfRepository.UpdateAsync(shelf);
                await _shelfRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<ShelfDTO>(shelf)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> DeleteShelfAsync(int id)
        {
            try
            {
                var shelf = await _shelfRepository.GetByIdAsync(id);
                if (shelf == null)
                    return new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = { "Shelf not found" } };

                await _shelfRepository.DeleteAsync(shelf);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Shelf delete successfully",
                        shelfID = shelf.ShelfId,
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
    }
}
