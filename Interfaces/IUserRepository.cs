using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<User> Users { get; set; }
        public Task<User> GetById(int id);
        public Task<bool> CheckIfUserExsits(int id);
        public Task<Cart> GetUserCart(int id);
        public Task<List<User>> GetByFilter(Expression<Func<User, bool>> filter);
        public Task<List<User>> GetAll();
        public Task<User> GetByEmail(string email);
        public Task Add(User entity);
        public Task<bool> Update(int id,User User);
        public Task<bool> Delete(int id);
    }
}