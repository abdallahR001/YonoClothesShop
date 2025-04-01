using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Text { get; set; }
        public int ProductId { get; set; }
        public int Rate { get; set; }
    }
}