using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class CartItemsRepository : ICartItemRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<CartItem> CartItems { get; set; }
        public CartItemsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            CartItems = _dbContext.CartItems;
        }
        public async Task<bool> Add(CartItem cartItem)
        {
            var exsistingCartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartId == cartItem.CartId && c.ProductId == cartItem.ProductId);
            
            if(exsistingCartItem == null)
            {
                await _dbContext.CartItems.AddAsync(cartItem);
                return true;
            }

            return false;
        }
        public async Task<bool> Delete(int id)
        {
            var cartItem = await _dbContext.CartItems.FindAsync(id);

            if(cartItem == null)
                return false;

            _dbContext.Remove(cartItem);
            return true;
        }

        public bool DeleteRange(List<CartItem> cartItems)
        {
            if(!cartItems.Any())
                return false;
            _dbContext.CartItems.RemoveRange(cartItems);
            return true;
        }
    }
}