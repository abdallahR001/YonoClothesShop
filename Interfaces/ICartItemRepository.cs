using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ICartItemRepository
    {
        public IQueryable<CartItem> CartItems { get; set; }
        public Task<bool> Add(CartItem cartItem);
        public Task<bool> Delete(int id);
        public bool DeleteRange(List<CartItem> cartItems);
    }
}