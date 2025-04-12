using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ICartRepository
    {
        public IQueryable<Cart> Carts { get; set; }
        public Task<bool> Add(Cart cart);
        public Task<bool> Delete(int id);
        public Task<Cart> GetById(int id);
        public Task<Cart> GetCartWithCartItems(int userId);
    }
}