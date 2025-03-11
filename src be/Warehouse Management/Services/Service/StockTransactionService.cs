using AutoMapper;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Helpers.Enum;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.StockTransaction;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class StockTransactionService : IStockTransactionService
    {

        private readonly IStockTransactionRepository _transactionRepo;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly ILotService _lotService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

        public StockTransactionService(
            IStockTransactionRepository transactionRepo,
            IProductService productService,
            ISupplierService supplierService,
            ILotService lotService,
            IMapper mapper,
            ILogger<StockTransactionService> logger,
            IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _transactionRepo = transactionRepo;
            _productService = productService;
            _supplierService = supplierService;
            _lotService = lotService;
            _mapper = mapper;
            _logger = logger;
            _exceptionHandlers = exceptionHandlers;
        }

        public async Task<ApiResponse> CreateTransactionAsync(CreateStockTransactionDTO dto, string userId)
        {
            try
            {
                // Lấy thông tin từ Service thay vì Repository
                var lotResponse = await _lotService.GetLotByIdAsync(dto.LotId);
                if (!lotResponse.IsSuccess)
                    throw new KeyNotFoundException($"Lô hàng với ID {dto.LotId} không tồn tại.");

                var productResponse = await _productService.GetProductByIdAsync(dto.ProductId);
                if (!productResponse.IsSuccess)
                    throw new KeyNotFoundException($"Sản phẩm với ID {dto.ProductId} không tồn tại.");

                var supplierResponse = await _supplierService.GetSupplierByIdAsync(dto.SupplierId);
                if (!supplierResponse.IsSuccess)
                    throw new KeyNotFoundException($"Nhà cung cấp với ID {dto.SupplierId} không tồn tại.");

                var lot = lotResponse.Result as Lot;
                var product = productResponse.Result as Product;
                var supplier = supplierResponse.Result as Supplier;

                // Kiểm tra số lượng nếu xuất kho
                if (dto.Type == TransactionType.Export && lot.Quantity < dto.Quantity)
                {
                    throw new InvalidOperationException($"Số lượng hàng trong kho không đủ. Yêu cầu: {dto.Quantity}, Tồn kho: {lot.Quantity}.");
                }

                // Tạo giao dịch kho
                var transaction = _mapper.Map<StockTransaction>(dto);
                transaction.UserId = userId;
                transaction.TransactionDate = DateTime.UtcNow;

                await _transactionRepo.AddTransactionAsync(transaction);

                // Cập nhật số lượng lô hàng thông qua `LotService`
                await _lotService.UpdateLotQuantityAsync(dto.LotId, dto.Type == TransactionType.Import ? dto.Quantity : -dto.Quantity);

                await _transactionRepo.SaveChangesAsync();

                _logger.LogInformation("Giao dịch kho đã tạo thành công: Loại: {Type}, Sản phẩm: {ProductId}, Lô: {LotId}, Số lượng: {Quantity}",
                    dto.Type, dto.ProductId, dto.LotId, dto.Quantity);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = _mapper.Map<StockTransactionDTO>(transaction)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepo.GetTransactionByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Không tìm thấy giao dịch với ID {id}.");

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<StockTransactionDTO>(transaction)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetTransactionsAsync(TransactionFilterDTO filter)
        {
            try
            {
                var transactions = await _transactionRepo.GetTransactionsAsync(filter);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new
                    {
                        Items = _mapper.Map<IEnumerable<StockTransactionDTO>>(transactions.ToList()), // 🛠 Dùng ToList() // ✅ Chỉ ánh xạ danh sách Items
                        TotalCount = transactions.TotalCount,
                        PageSize = transactions.PageSize,
                        CurrentPage = transactions.CurrentPage,
                        TotalPages = transactions.TotalPages
                    }
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetTransactionsByLotIdAsync(int lotId)
        {
            try
            {
                var transactions = await _transactionRepo.GetTransactionsByLotIdAsync(lotId);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<IEnumerable<StockTransactionDTO>>(transactions)
                };
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        private async Task<ApiResponse> HandleExceptionAsync(Exception ex)
        {
            _logger.LogError(ex, "Lỗi xảy ra trong StockTransactionService");

            var context = new DefaultHttpContext();
            foreach (var handler in _exceptionHandlers)
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
                ErrorMessages = { "Đã xảy ra lỗi không mong muốn trong hệ thống." }
            };
        }
    }
}