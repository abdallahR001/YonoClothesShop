using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [Required, MaxLength(300)]
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? category { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}