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
        public Task<List<ProductDTO>> GetProductsByCategory(int id);
        public Task<List<ProductDTO>> GetProductsByName(string Name);
        public Task<List<ProductDTO>> GetProductsFiltredByPrice(int minPrice, int? maxPrice = null);
        public Task<int> AddProduct(string name, string description, IFormFile image, int price, int count, int categoryId);
        public Task<int> UpdateProduct(int id, string name, string description, IFormFile image, int price, int count);
    }
}