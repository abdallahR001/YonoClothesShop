using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ProfileImage { get; set; }
        public int OrdersCount { get; set; }
        public int Amount { get; set; }
        public string? Address { get; set; }
    }
}