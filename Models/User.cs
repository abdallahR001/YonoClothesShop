using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string? Name { get; set; }
        [Required,MaxLength(50)]
        public string? Email { get; set; }
        [Required,MaxLength(100)]
        public string? PasswordHash { get; set; }
        [Required,MaxLength(50)]
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public int Amount { get; set; }
        public int OrdersCount { get; set; }
        public Cart? cart { get; set; }
        public List<Review>? reviews { get; set; }
    }
}