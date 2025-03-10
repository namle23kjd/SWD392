﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Management.Models.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("Platform")]
        public int PlatformId { get; set; }
        public string? PlatformOrderId { get; set; }
        public bool OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public Platform? Platform { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
