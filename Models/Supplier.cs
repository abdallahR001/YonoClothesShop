using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public int PhoneNumber { get; set; }
        public int DeleveriesCount { get; set; }
        public int TotalDeleveriesPrice { get; set; }
    }
}