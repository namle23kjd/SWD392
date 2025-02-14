﻿using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO.Order;

namespace Warehouse_Management.Services.IService
{
    public interface IOrderService
    {
        Task<ApiResponse> GetAllOrdersAsync();
        Task<ApiResponse> GetOrderByIdAsync(int id);
        Task<ApiResponse> CreateOrderAsync(CreateOrderDTO orderDto);
        Task<ApiResponse> DeleteOrderAsync(int id);
        Task<ApiResponse> UpdateOrderAsync(int id, UpdateOrderDTO orderDto);
        Task<ApiResponse> HandlerExceptionAsync(Exception ex);
    }
}
