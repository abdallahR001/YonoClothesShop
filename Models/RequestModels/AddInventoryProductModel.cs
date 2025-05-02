using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class AddInventoryProductModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierCompany { get; set; }
        public int SupplierPrice { get; set; }
    }
}