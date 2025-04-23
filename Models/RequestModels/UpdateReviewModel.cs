using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models.RequestModels
{
    public class UpdateReviewModel
    {
        public string? Review { get; set; }
        public int Rating { get; set; }
    }
}