using AutoMapper;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Supplier;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
        private readonly ILogger<SupplierService> _logger;
        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper, IEnumerable<IExceptionHandler> exceptionHandlers, ILogger<SupplierService> logger)
        {
            _exceptionHandlers = exceptionHandlers;
            _logger = logger;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateSupplierAsync(CreateSupplierDTO dto)
        {
            try
            {
                if(await _supplierRepository.GetByEmailAsync(dto.Email) != null)
                    throw new BadHttpRequestException($"Email {dto.Email} is already taken");

                var supplier = _mapper.Map<Supplier>(dto);
                supplier.CreateAt = DateTime.UtcNow;

                await _supplierRepository.CreateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Result = _mapper.Map<SupplierDTO>(supplier)
                };

            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllSuppliersAsync()
        {
            try
            {
                var suppliers = await _supplierRepository.GetAllAsync();
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = _mapper.Map<IEnumerable<SupplierDTO>>(suppliers),
                };
            }
            catch(Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetSupplierByIdAsync(int id)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"No supplier found with ID {id}");
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = _mapper.Map<SupplierDTO>(supplier)
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
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ErrorMessages = { "Internal Server Error" },
            };
        }

        public async Task<ApiResponse> UpdateSupplierAsync(int id, UpdateSupplierDTO dto)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"No supplier found with ID {id}");
                _mapper.Map(dto, supplier);

                await _supplierRepository.UpdateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = _mapper.Map<SupplierDTO>(supplier)
                };

            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);

            }
        }
    }
}
