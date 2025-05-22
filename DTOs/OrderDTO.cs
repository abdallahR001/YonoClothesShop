using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductsCount { get; set; }
        public string? Status { get; set; }
        public List<OrderItemDTO>? OrderItems { get; set; } = new List<OrderItemDTO>();
        public string? PaymentMethod { get; set; }
    }
}