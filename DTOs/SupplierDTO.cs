using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public int DeleveriesCount { get; set; }
        public int TotalDeleveriesPrice { get; set; }
    }
}