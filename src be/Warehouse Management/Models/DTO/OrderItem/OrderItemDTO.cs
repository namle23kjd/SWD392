﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.DTO.OrderItem
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
