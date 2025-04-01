using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YonoClothesShop.Repository
{
    public interface IbaseRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll();
        Task<bool> GetByEmail(string email);
        Task Add(T entity);
        void Update(T entity);
        Task Delete(int id);
    }
}