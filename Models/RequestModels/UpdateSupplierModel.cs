using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class UpdateSupplierModel
    {
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}