using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Repositories.Repository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExceptionHandler> _exceptionhandlers;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILotRepository _lotRepository;
        private readonly IShelfRepository _shelfRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IMapper mapper, IProductRepository productRepository, IEnumerable<IExceptionHandler> exceptionHandlers, UserManager<IdentityUser> userManager, ILotRepository lotRepository, IStockTransactionRepository stockTransactionRepository, IShelfRepository shelfRepository, ILogger<ProductService> logger, ISupplierRepository supplierRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _exceptionhandlers = exceptionHandlers;
            _userManager = userManager;
            _lotRepository = lotRepository;
            _supplierRepository = supplierRepository;
            _stockTransactionRepository = stockTransactionRepository;
            _shelfRepository = shelfRepository;
            _logger = logger;
        }


        public async Task<ApiResponse> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Product not found" }
                    };
                }

                // Lấy thông tin user
                var user = await _userManager.FindByIdAsync(product.UserId);
                var productDto = _mapper.Map<ProductDTO>(product);
                productDto.UserName = user != null ? user.UserName : "Unknown"; // Gán UserName

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = productDto
                };
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

            var userIds = products.Select(p => p.UserId).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.UserName);

            var productDtos = _mapper.Map<List<ProductDTO>>(products);

            // Gán tên người dùng vào DTO
            foreach (var productDto in productDtos)
            {
                productDto.UserName = users.ContainsKey(productDto.UserName) ? users[productDto.UserName] : "Unknown";
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
                var product = _mapper.Map<Product>(productDto);
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var nowInVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                product.CreatedAt = nowInVietnam;
                product.UpdatedAt = nowInVietnam;
                product.UserId = userId;

                await _productRepository.AddProductAsync(product);
                await _productRepository.SaveChangesAsync();
                var productResponse = _mapper.Map<ProductDTO>(product);
                productResponse.UserName = user.UserName; // Gán UserName trước khi trả về

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = productResponse
                };
            }
            catch (Exception ex)
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
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Product not found" }
                    };

                _mapper.Map(productDto, product);
                product.UpdatedAt = DateTime.UtcNow;

                _productRepository.UpdateProductAsync(product);
                await _productRepository.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(product.UserId);
                var productResponse = _mapper.Map<ProductDTO>(product);
                productResponse.UserName = user?.UserName ?? "Unknown";  // Gán UserName vào DTO

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<ProductDTO>(product)
                };
            }
            catch (Exception ex)
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

                return new ApiResponse 
                { 
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Message = "Product delete successfully",
                        productID = product.ProductId,
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

        public async Task<ApiResponse> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pagedProducts = await _productRepository.GetALlProductsAsync(pageNumber, pageSize);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        TotalCount = pagedProducts.TotalCount,
                        PageSize = pagedProducts.PageSize,
                        CurrentPage = pagedProducts.CurrentPage,
                        TotalPages = pagedProducts.TotalPages,
                        Products = _mapper.Map<IEnumerable<ProductDTO>>(pagedProducts)
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> CreateProductFromTransactionAsync(CreateProductFromTransaction importDto)
        {
            try
            {
                _logger.LogInformation($"Fetching Supplier with ID: {importDto.SupplierId}");
                var supplier = await _supplierRepository.GetByIdAsync(importDto.SupplierId);
                if (supplier == null)
                {
                    _logger.LogError($"Supplier with ID {importDto.SupplierId} not found.");
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { $"Supplier with ID {importDto.SupplierId} not found." }
                    };
                }

                // ✅ Kiểm tra nếu Product đã tồn tại theo SKU
                var product = await _productRepository.GetProductBySKUAsync(importDto.SKU);
                if (product == null)
                {
                    product = new Product
                    {
                        SKU = importDto.SKU,
                        Barcode = importDto.Barcode,
                        ProductName = importDto.ProductName,
                        Description = importDto.Description,
                        BasePrice = importDto.BasePrice,
                        CreatedAt = DateTime.UtcNow,
                        UserId = importDto.UserId
                    };

                    await _productRepository.AddProductAsync(product);
                    await _productRepository.SaveChangesAsync();
                }

                // ✅ Kiểm tra nếu Shelf tồn tại (Vì Lot cần có ShelfId)
                var shelf = await _shelfRepository.GetByIdAsync(importDto.ShelfId);
                if (shelf == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { $"Shelf with ID {importDto.ShelfId} not found." }
                    };
                }

                // ✅ Kiểm tra nếu Lot đã tồn tại cho Product
                var lot = await _lotRepository.GetByProductIdAsync(product.ProductId);
                if (lot == null)
                {
                    // 🔥 Nếu chưa có Lot, tạo mới Lot
                    lot = new Lot
                    {
                        ProductId = product.ProductId,
                        ShelfId = shelf.ShelfId,
                        LotCode = $"LOT-{Guid.NewGuid().ToString().Substring(0, 8)}",
                        Quantity = 0,  // ✅ Sẽ cập nhật sau
                        ManufactureDate = importDto.ManufactureDate,
                        ExpiryDate = DateTime.UtcNow.AddYears(1),
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        UpdateAt = DateTime.UtcNow,
                        UserId = importDto.UserId
                    };

                    await _lotRepository.CreateAsync(lot);
                    await _lotRepository.SaveChangesAsync();
                }

                // ✅ Tạo `StockTransaction`
                var stockTransaction = new StockTransaction
                {
                    ProductId = product.ProductId,
                    SupplierId = supplier.SupplierId,
                    LotId = lot.LotId,
                    UserId = importDto.UserId,
                    Quantity = importDto.Quantity,
                    Type = "Import",
                    TransactionDate = DateTime.UtcNow
                };

                await _stockTransactionRepository.AddTransactionAsync(stockTransaction);
                await _stockTransactionRepository.SaveChangesAsync();

                // ✅ Cập nhật số lượng trong Lot
                lot.Quantity += importDto.Quantity;
                await _lotRepository.UpdateAsync(lot);
                await _lotRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = new
                    {
                        Message = "Product created and stock imported successfully!",
                        ProductId = product.ProductId,
                        LotId = lot.LotId,
                        TransactionId = stockTransaction.TransactionId
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