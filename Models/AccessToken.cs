using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Models
{
    public class AccessToken
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? user { get; set; }
        public DateTime Expiration { get; set; }
    }
}