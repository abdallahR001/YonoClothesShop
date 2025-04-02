using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? user { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}