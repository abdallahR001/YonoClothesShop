using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public Task<Product> GetProduct(int id);
        public Task<List<ProductDTO>> GetProductsByCategory(string categoryName);
        public Task<List<ProductDTO>> GetProductsByName(string Name);
        public Task<List<ProductDTO>> GetProductsFiltredByPrice(int minPrice, int? maxPrice = null);
        public Task<List<ProductDTO>> GetProductsByRating(Expression<Func<int,bool>> filter);
        public Task<List<ProductDTO>> GetProductsByPriceAndRating(Expression<Func<int,int,bool>> filter);
    }
}