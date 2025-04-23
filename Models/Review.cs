using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, MaxLength(200)]
        public string? Text { get; set; }
        public int ProductId { get; set; }
        public int? Rate { get; set; }
        public User? user { get; set; }
        public Product? product { get; set; }
    }
}