using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class UpdateUserProfileModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}