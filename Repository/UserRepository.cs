using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Net.Http.Headers;
using YonoClothesShop.Data;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<User> Users { get; set; }
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Users = _dbContext.Users;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return null;
            return user;
        }

        public async Task<List<User>> GetByFilter(Expression<Func<User, bool>> filter)
        {
            return await _dbContext.Users.Where(filter).ToListAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
                return null;
            return user;
        }

        public async Task Add(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
        }

        public async Task<bool> Update(int id,User User)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if(user == null)
                return false;

            user.Name = User.Name;

            user.Email = User.Email;

            user.Address = User.Address;

            _dbContext.Users.Update(user);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await GetById(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                return true;
            }
            return false;
        }

        public async Task<Cart> GetUserCart(int id)
        {
            var cart = await _dbContext.Carts
            .Include(c => c.cartItems)
            .FirstOrDefaultAsync(c => c.UserId == id);

            if(cart == null)
                return null;

            return cart;
        }

        public async Task<bool> CheckIfUserExsits(int id)
        {
            var user = await _dbContext.Users.AnyAsync(u => u.Id == id);

            if(!user)
                return false;

            return true;
        }
    }
}