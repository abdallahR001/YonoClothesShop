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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Cart> Carts { get; set; }
        public CartRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Carts = _dbContext.Carts;
        }

        public async Task<bool> Add(Cart cart)
        {
            var exsistingCart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == cart.UserId);

            if(exsistingCart == null)
            {
                await _dbContext.Carts.AddAsync(cart);
                return true;
            }

            return false;     
        }

        public async Task<bool> Delete(int id)
        {
            var cart = await _dbContext.Carts.FindAsync(id);

            if(cart == null)
                return false;

            _dbContext.Remove(cart);
            return true;
        }

        public async Task<Cart> GetById(int id)
        {
            var cart = await _dbContext.Carts.FindAsync(id);

            if(cart == null)
                return null;

            return cart;
        }

        public async Task<Cart> GetCartWithCartItems(int userId)
        {
            var cart = await _dbContext.Carts
            .Include(c => c.cartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
                return null;
            
            return cart;
        }
    }
}