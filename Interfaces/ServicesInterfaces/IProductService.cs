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
        public Task<List<ProductDTO>> GetProducts();
        public Task<List<ProductDTO>> GetProductsByCategory(int id);
        public Task<List<ProductDTO>> GetProductsByName(string Name);
        public Task<List<ProductDTO>> GetProductsFiltredByPrice(int categoryId, int minPrice, int? maxPrice = null);
        public Task<ProductDTO> GetProductWithReviews(int id);
        public Task<int> AddProduct(int productId, int count);
        public Task<bool> Delete(int id);
    }
}