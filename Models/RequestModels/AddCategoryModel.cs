using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class AddCategoryModel
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}