using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public interface IbaseRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(int id, T entity);
        Task<bool> Delete(int id);
    }
}