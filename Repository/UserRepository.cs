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
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class UserRepository : IbaseRepository<User>
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<User> Users;
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
            await _dbContext.Users.AddAsync(entity);;
        }

        public async Task<User> Update(int id,User User)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user == null)
                return null;
            user.Name = User.Name;
            user.Email = User.Email;
            user.Address = User.Address;
            user.Name = User.Name;
            _dbContext.Users.Update(user);
            return user;
        }

        public async Task Delete(int id)
        {
            var user = await GetById(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
            }
        }

    }
}