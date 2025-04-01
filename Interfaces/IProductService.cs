using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProduct(int id);
        public Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
        public Task<IEnumerable<Product>> GetProductsByName(string Name);
        public Task<IEnumerable<Product>> GetProductsByPrice(Expression<Func<int,bool>> filter);
        public Task<IEnumerable<Product>> GetProductsByRating(Expression<Func<int,bool>> filter);
        public Task<IEnumerable<Product>> GetProductsByPriceAndRating(Expression<Func<int,int,bool>> filter);
    }
}