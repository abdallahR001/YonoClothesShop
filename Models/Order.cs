using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductsCount { get; set; }
        public string? Status { get; set; }
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public string? PaymentMethod { get; set; }
    }
}