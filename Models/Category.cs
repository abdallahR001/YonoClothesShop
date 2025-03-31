using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(30)]
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int ProductsCount { get; set; }
        public List<Product>? Products { get; set; }
    }
}