using AutoMapper;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExceptionHandler> _exceptionhandlers;

        public ProductService(IMapper mapper, IProductRepository productRepository, IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _exceptionhandlers = exceptionHandlers;
        }
        public async Task<ApiResponse> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetALlProductsAsync();
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<IEnumerable<ProductDTO>>(products)
                };
            }
            catch(Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                    return new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = { "Product not found" } };

                return new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = _mapper.Map<ProductDTO>(product) };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> SearchProductsAsync(string? sku, string? barcode, string? name)
        {
                var products = await _productRepository.SearchProductAsync(sku, barcode, name);
            if (products == null || !products.Any()) // Nếu không tìm thấy kết quả
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "No products found matching the search criteria." }
                };
            }

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<IEnumerable<ProductDTO>>(products)
            };

        }

        public async Task<ApiResponse> CreateProductAsync(CreateProductDTO productDto, string userId)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var nowInVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                product.CreatedAt = nowInVietnam;
                product.UpdatedAt = nowInVietnam;
                product.UserId = userId;

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = _mapper.Map<ProductDTO>(product) };
            }catch(Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> UpdateProductAsync(int id, UpdateProductDTO productDto)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                    return new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = { "Product not found" } };

                _mapper.Map(productDto, product);
                product.UpdatedAt = DateTime.UtcNow;

                _productRepository.UpdateProductAsync(product);
                await _productRepository.SaveChangesAsync();

                return new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.NoContent };
            }
            catch(Exception ex)
            {
                   return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                    return new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = { "Product not found" } };

                await _productRepository.DeleteProductAsync(product);

                return new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.NoContent };
            } catch(Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> HandleExceptionAsync(Exception ex)
        {
            var context = new DefaultHttpContext();
            foreach (var handler in _exceptionhandlers)
            {
                if (await handler.TryHandleAsync(context, ex, CancellationToken.None))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (HttpStatusCode)context.Response.StatusCode,
                        ErrorMessages = { ex.Message }
                    };
                }
            }
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = { ex.Message }
            };
        }
    }
}