using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Repositories.Repository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILotRepository _lotRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IStockTransactionRepository _stockTransactionRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IEnumerable<IExceptionHandler> exceptionHandlers, ILogger<OrderService> logger, ILotRepository lotRepository, IProductRepository productRepository, IStockTransactionRepository stockTransactionRepository, UserManager<IdentityUser> userManager)
        {
            _exceptionHandlers = exceptionHandlers;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lotRepository = lotRepository;
            _productRepository = productRepository;
            _stockTransactionRepository = stockTransactionRepository;
            _userManager = userManager;
        }

        public async Task<ApiResponse> CreateOrderAsync(CreateOrderDTO orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.CreatedAt = DateTime.UtcNow;
                order.OrderStatus = true;
                decimal totalAmount = 0;

                // 🚀 Đảm bảo OrderItems không bị null
                order.OrderItems = new List<OrderItem>();

                _logger.LogInformation("🚀 Starting CreateOrderAsync...");
                Dictionary<int, int> productQuantityCheck = new Dictionary<int, int>();

                _logger.LogInformation("📌 Dictionary initialized successfully.");

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var user = await _userManager.FindByIdAsync(orderDto.UserId);
                    if (user == null)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { "User not found" }
                        };
                    }
                    var product = await _productRepository.GetProductByIdAsync(itemDto.ProductId);
                    if (product == null)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"Product ID {itemDto.ProductId} does not exist." }
                        };
                    }
                    if (itemDto.Quantity <= 0)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"Quantity must be greater than 0 for Product ID {itemDto.ProductId}." }
                        };
                    }
                    var lotProduct = await _lotRepository.GetByProductIdAsync(itemDto.ProductId);
                    if (lotProduct == null)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"⚠️ Product ID {itemDto.ProductId} is not available in any stock. Please check inventory!" }
                        };
                    }


                    // 🔹 Lấy giá UnitPrice từ Product
                    decimal unitPrice = product.BasePrice ?? 0;

                    // 🔹 Kiểm tra tồn kho Lot
                    var lot = await _lotRepository.GetByProductIdAsync(itemDto.ProductId);
                    if (lot == null || lot.Quantity < itemDto.Quantity)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { $"Not enough stock for Product ID {itemDto.ProductId}" }
                        };
                    }

                    // 🔹 Trừ kho Lot
                    lot.Quantity -= itemDto.Quantity;
                    await _lotRepository.UpdateAsync(lot);

                    // 🔹 Tạo OrderItem và gán UnitPrice
                    var orderItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = unitPrice // ✅ Gán giá từ Product.BasePrice
                    };

                    order.OrderItems.Add(orderItem); // ✅ Thêm vào OrderItems của Order
                    totalAmount += orderItem.Quantity * orderItem.UnitPrice;

                    var stockTransaction = new StockTransaction
                    {
                        ProductId = itemDto.ProductId,
                        SupplierId = null, // Có thể để SupplierId là null nếu không cần
                        LotId = lot.LotId,
                        UserId = orderDto.UserId, // Gán UserId từ Order DTO
                        Quantity = itemDto.Quantity,
                        Type = "Export", // Đặt type là "Export" vì đây là xuất kho
                        TransactionDate = DateTime.UtcNow // Gán thời gian giao dịch là thời gian hiện tại
                    };


                    // Lưu StockTransaction
                    await _stockTransactionRepository.AddTransactionAsync(stockTransaction);
                    await _stockTransactionRepository.SaveChangesAsync();
                }

                // 🔹 Gán tổng tiền đơn hàng
                order.TotalAmount = totalAmount;

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangesAsync();

                var orderResponse = _mapper.Map<OrderDTO>(order);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = orderResponse  
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }


        public async Task<ApiResponse> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Order with ID {id} not found" }
                    };
                }

                // ✅ Đánh dấu OrderStatus là false thay vì xóa cứng
                order.OrderStatus = false;

                // ✅ Hoàn trả số lượng sản phẩm về Lot
                foreach (var orderItem in order.OrderItems)
                {
                    var lot = await _lotRepository.GetByProductIdAsync(orderItem.ProductId);
                    if (lot != null)
                    {
                        // 🔥 Cộng lại số lượng vào Lot
                        lot.Quantity += orderItem.Quantity;
                        await _lotRepository.UpdateAsync(lot);
                    }
                }

                await _orderRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK, // ✅ Trả về 200 OK thay vì 204 NoContent
                    Result = new { Message = "Order successfully deleted (soft delete). Inventory restored." }
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> DeleteOrderItemAsync(int orderId, int orderItemId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"Order with ID {orderId} not found" }
                    };
                }

                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
                if (orderItem == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { $"OrderItem with ID {orderItemId} not found in Order {orderId}" }
                    };
                }

                // ✅ Hoàn trả số lượng về Lot
                var lot = await _lotRepository.GetByProductIdAsync(orderItem.ProductId);
                if (lot != null)
                {
                    lot.Quantity += orderItem.Quantity;
                    await _lotRepository.UpdateAsync(lot);
                }

                // ✅ Xóa OrderItem khỏi Order
                order.OrderItems.Remove(orderItem);

                // ✅ Nếu Order không còn OrderItem nào, tự động xóa luôn Order
                if (!order.OrderItems.Any())
                {
                    order.OrderStatus = false; // Chuyển trạng thái Order về False
                }
                else
                {
                    // ✅ Cập nhật lại tổng giá trị đơn hàng
                    order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
                }

                await _orderRepository.SaveChangesAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = new { Message = $"OrderItem {orderItemId} deleted from Order {orderId}. Inventory restored." }
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetAllOrdersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pagedOrders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);

                var response = new
                {
                    TotalCount = pagedOrders.TotalCount,
                    PageSize = pagedOrders.PageSize,
                    CurrentPage = pagedOrders.CurrentPage,
                    TotalPages = pagedOrders.TotalPages,
                    Orders = _mapper.Map<IEnumerable<OrderDTO>>(pagedOrders)
                };

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<OrderDTO>(order)
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }

        public async Task<ApiResponse> HandlerExceptionAsync(Exception ex)
        {
            _logger.LogError(ex, "Error occurred in OrderService");

            var context = new DefaultHttpContext(); // Fake HttpContext để xử lý lỗi trong Service
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
                ErrorMessages = { "An unexpected error occurred." }
            };
        }

        public async Task<ApiResponse> UpdateOrderAsync(int id, UpdateOrderDTO orderDto)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Order not found" }
                    };
                }

                // ✅ Cập nhật trạng thái đơn hàng
                order.OrderStatus = orderDto.OrderStatus;

                // ✅ Cập nhật số lượng OrderItem
                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    var existingItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemDto.OrderItemId);

                    if(existingItem == null)
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.NotFound,
                            ErrorMessages = { $"OrderItem with ID {orderItemDto.OrderItemId} not found in Order" }
                        };
                    }

                    if (existingItem != null)
                    {
                        // 🔥 Kiểm tra tồn kho Lot
                        var lot = await _lotRepository.GetByProductIdAsync(existingItem.ProductId);
                        if (lot == null || lot.Quantity + existingItem.Quantity < orderItemDto.Quantity)
                        {
                            return new ApiResponse
                            {
                                IsSuccess = false,
                                StatusCode = HttpStatusCode.BadRequest,
                                ErrorMessages = { $"Not enough stock for Product ID {existingItem.ProductId}" }
                            };
                        }

                        // 🔥 Cập nhật số lượng trong OrderItem
                        int quantityDiff = orderItemDto.Quantity - existingItem.Quantity;
                        existingItem.Quantity = orderItemDto.Quantity;

                        // 🔥 Cập nhật tồn kho Lot
                        lot.Quantity -= quantityDiff;
                        await _lotRepository.UpdateAsync(lot);
                    }
                }

                // ✅ Cập nhật tổng tiền đơn hàng
                order.TotalAmount = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);

                await _orderRepository.SaveChangesAsync();

                var orderResponse = _mapper.Map<OrderDTO>(order);
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = orderResponse
                };
            }
            catch (Exception ex)
            {
                return await HandlerExceptionAsync(ex);
            }
        }
    }
}
