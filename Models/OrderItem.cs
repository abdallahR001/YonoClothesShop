using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string? ProductImage { get; set; }
        
    }
}