using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int SupplierId { get; set; }
        public Supplier? supplier { get; set; }
        public int SupplierPrice { get; set; }
        public int SoldCount { get; set; }
        public DateTime AddedAt { get; set; }
    }
}