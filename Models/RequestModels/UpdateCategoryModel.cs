using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class UpdateCategoryModel
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}