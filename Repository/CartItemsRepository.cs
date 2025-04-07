using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class CartItemsRepository : IbaseRepository<CartItem>
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<CartItem> CartItems;
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

        public async Task<CartItem> GetById(int id)
        {
            var cartItem = await _dbContext.CartItems.FindAsync(id);

            if(cartItem == null)
                return null;

            return cartItem;
        }

        public Task<bool> Update(int id, CartItem entity)
        {
            throw new NotImplementedException();
        }
    }
}