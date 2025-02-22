using AutoMapper;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IEnumerable<IExceptionHandler> exceptionHandlers, ILogger<OrderService> logger)
        {
            _exceptionHandlers = exceptionHandlers;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<ApiResponse> CreateOrderAsync(CreateOrderDTO orderDto)
        {
            try
            {
               
                var order = _mapper.Map<Order>(orderDto);
                order.CreatedAt = DateTime.UtcNow;

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
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                await _orderRepository.DeleteOrderAsync(order);
                await _orderRepository.SaveChangesAsync();

                return new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.NoContent };
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
                var orders = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<IEnumerable<OrderDTO>>(orders)
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
                _mapper.Map(orderDto, order);

                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    var orderItem = order.OrderItems.FirstOrDefault(x => x.OrderItemId == orderItemDto.OrderItemId);
                    if (orderItem != null)
                    {
                        _mapper.Map(orderItemDto, orderItem); // Ánh xạ cập nhật OrderItem
                    }
                    else
                    {
                        // Nếu không tìm thấy OrderItem trong đơn hàng hiện tại, có thể thêm mới hoặc xử lý tùy theo logic của bạn.
                        var newOrderItem = _mapper.Map<OrderItem>(orderItemDto);
                        order.OrderItems.Add(newOrderItem);
                    }
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _orderRepository.SaveChangesAsync();

                // Trả về dữ liệu đơn hàng đã cập nhật
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
