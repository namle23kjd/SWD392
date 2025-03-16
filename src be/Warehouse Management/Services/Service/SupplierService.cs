using AutoMapper;
using System.Net;
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
                // Kiểm tra nếu email đã tồn tại trong cơ sở dữ liệu
                var existingSupplierByEmail = await _supplierRepository.GetByEmailAsync(dto.Email);
                if (existingSupplierByEmail != null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { $"Email '{dto.Email}' is already used by another provider." }
                    };
                }

                // Kiểm tra nếu số điện thoại đã tồn tại trong cơ sở dữ liệu
                var existingSupplierByPhone = await _supplierRepository.GetByPhoneAsync(dto.Phone);
                if (existingSupplierByPhone != null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { $"Phone number '{dto.Phone}' is already used by another provider." }
                    };
                }

                // Nếu email và số điện thoại chưa tồn tại, tiến hành tạo mới nhà cung cấp
                var supplier = _mapper.Map<Supplier>(dto);
                supplier.CreateAt = DateTime.UtcNow;

                await _supplierRepository.CreateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = _mapper.Map<SupplierDTO>(supplier)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }


        public async Task<ApiResponse> DeleteSupplierAsync(int id)
        {
            try
            {
                // Tìm nhà cung cấp theo ID
                var supplier = await _supplierRepository.GetByIdAsync(id);

                if (supplier == null)
                    throw new BadHttpRequestException($"Supplier with ID {id} not found");

                // Xóa nhà cung cấp khỏi cơ sở dữ liệu
                await _supplierRepository.DeleteAsync(id);
                await _supplierRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Supplier delete successfully",
                        supplierId = supplier.SupplierId
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllSuppliersAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var (suppliers, totalCount) = await _supplierRepository.GetAllAsync(page, pageSize);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        TotalCount = totalCount,
                        Suppliers = _mapper.Map<IEnumerable<SupplierDTO>>(suppliers)
                    }
                };
            }
            catch (Exception ex)
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

                // Kiểm tra trùng email
                if (supplier.Email != dto.Email)
                {
                    var existingSupplierByEmail = await _supplierRepository.GetByEmailAsync(dto.Email);
                    if (existingSupplierByEmail != null && existingSupplierByEmail.SupplierId != id)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"Email '{dto.Email}' is already used by another provider." }
                        };
                    }
                }

                // Kiểm tra trùng số điện thoại
                if (supplier.Phone != dto.Phone)
                {
                    var existingSupplierByPhone = await _supplierRepository.GetByPhoneAsync(dto.Phone);
                    if (existingSupplierByPhone != null && existingSupplierByPhone.SupplierId != id)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"Phone number '{dto.Phone}' is already used by another provider." }
                        };
                    }
                }

                _mapper.Map(dto, supplier);

                await _supplierRepository.UpdateAsync(supplier);
                await _supplierRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
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
