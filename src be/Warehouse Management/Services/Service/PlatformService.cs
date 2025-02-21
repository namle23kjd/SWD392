using AutoMapper;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Platform;
using Warehouse_Management.Models.DTO.PlatformDTO;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepo;
        private readonly IMapper _mapper;

        public PlatformService(IPlatformRepository platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        public async Task<ApiResponse> GetPlatformByIdAsync(int id)
        {
            var platform = await _platformRepo.GetPlatformByIdAsync(id);

            if (platform == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Platform not found" }
                };
            }

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<PlatfromDTO>(platform)
            };
        }

        public async Task<ApiResponse> GetAllPlatformsAsync()
        {
            var platforms = await _platformRepo.GetAllPlatformsAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<IEnumerable<PlatfromDTO>>(platforms)
            };
        }

        public async Task<ApiResponse> CreatePlatformAsync(CreatePlatformDTO platformDto)
        {
            var platform = _mapper.Map<Platform>(platformDto);
            await _platformRepo.AddPlatformAsync(platform);
            await _platformRepo.SaveChangesAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = _mapper.Map<PlatfromDTO>(platform)
            };
        }

        public async Task<ApiResponse> UpdatePlatformAsync(int id, UpdatePlatformDTO platformDto)
        {
            var platform = await _platformRepo.GetPlatformByIdAsync(id);
            if (platform == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Platform not found" }
                };
            }

            _mapper.Map(platformDto, platform);
            await _platformRepo.UpdatePlatformAsync(platform);
            await _platformRepo.SaveChangesAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<PlatfromDTO>(platform)
            };
        }

        public async Task<ApiResponse> DeletePlatformAsync(int id)
        {
            var platform = await _platformRepo.GetPlatformByIdAsync(id);
            if (platform == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Platform not found" }
                };
            }

            await _platformRepo.DeletePlatformAsync(id);
            await _platformRepo.SaveChangesAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent
            };
        }
    }
}
