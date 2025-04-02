using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int UserId { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}