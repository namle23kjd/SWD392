using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<IdentityUser> _userManager;

        public ShelfService(IShelfRepository shelfRepository, IMapper mapper, ILogger<ShelfService> logger, IEnumerable<IExceptionHandler> exceptionHandlers, UserManager<IdentityUser> userManager)
        {
            _exceptionHandlers = exceptionHandlers;
            _logger = logger;
            _mapper = mapper;
            _shelfRepository = shelfRepository;
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateShelfAsync(CreateShelfDTO dto, string userId)
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

                // Kiểm tra tính duy nhất của mã kệ (code) trước khi tạo mới
                var codeExists = await _shelfRepository.IsCodeUniqueAsync(dto.Code);
                if (!codeExists) // Sửa đổi điều kiện này để phản hồi khi mã kệ đã tồn tại
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { $"Shelf with the code '{dto.Code}' already exists." }
                    };
                }

                // Map DTO sang entity Shelf
                var shelf = _mapper.Map<Shelf>(dto);
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var nowInVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                shelf.CreatedAt = nowInVietnam;
                shelf.UpdatedAt = nowInVietnam;
                shelf.UserId = userId;
                shelf.IsActive = true;

                // Lưu kệ vào cơ sở dữ liệu
                await _shelfRepository.CreateAsync(shelf);
                await _shelfRepository.SaveChangesAsync();

                var shelfResponse = _mapper.Map<ShelfDTO>(shelf);
                shelfResponse.UserName = user.UserName; // Gán UserName vào DTO

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = shelfResponse
                };
            }
            catch (Exception ex)
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
                        Items = _mapper.Map<IEnumerable<ShelfDTO>>(shelves),
                        TotalCount = totalCount,
                        PageSize = pageSize,
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
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

                var user = await _userManager.FindByIdAsync(shelf.UserId);
                var shelfDto = _mapper.Map<ShelfDTO>(shelf);
                shelfDto.UserName = user != null ? user.UserName : "Unknown"; // Gán UserName vào DTO

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = shelfDto
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> UpdateShelfAsync(int id, UpdateShelfDTO dto)
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


                // Cập nhật giá trị từ dto
                _mapper.Map(dto, shelf);

                // Cập nhật thời gian sửa đổi
                shelf.UpdatedAt = DateTime.UtcNow;

                // Kiểm tra và cập nhật IsActive nếu có thay đổi trong dto
                if (dto.IsActive != null)
                {
                    shelf.IsActive = dto.IsActive.Value;
                }

                // Cập nhật Capacity nếu có thay đổi
                if (dto.Capacity.HasValue)
                {
                    shelf.Capacity = dto.Capacity.Value;
                }

                await _shelfRepository.UpdateAsync(shelf);
                await _shelfRepository.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(shelf.UserId);
                var shelfResponse = _mapper.Map<ShelfDTO>(shelf);
                shelfResponse.UserName = user?.UserName ?? "Unknown"; // Gán UserName vào DTO

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = shelfResponse
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
                {
                    return new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = { "Shelf not found" } };
                }

                await _shelfRepository.DeleteAsync(shelf);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Shelf deleted successfully",
                        shelfID = shelf.ShelfId,
                    }
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
    }
}
