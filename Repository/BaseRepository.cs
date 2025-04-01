using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class BaseRepository<T> : IbaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = await _dbContext.Set<T>().ToListAsync();
            return entities;
        }

        public async Task<bool> GetByEmail(string email)
        {
            var entity = await _dbContext.Set<User>().AsNoTracking().AnyAsync(e => e.Email == email);
            if (entity)
                return true;
            return false;
                
        }

        public async Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter)
        {
            var entities = await _dbContext.Set<T>().Where(filter).ToListAsync();
            return entities;
        }
        public async Task<T> GetById(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if(entity == null)
                return null;
            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}