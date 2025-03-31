using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        List<CartItem>? cartItems { get; set; } = new List<CartItem>();
    }
}