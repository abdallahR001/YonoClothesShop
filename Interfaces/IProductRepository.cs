using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IProductRepository
    {
        public IQueryable<Product> Products { get; set; }
        public Task<Product> GetById(int id);
        public Task<bool> CheckIfProductExsist(int id);
        public Task<bool> Add(Product product);
        public Task<bool> Delete(int id);
        public Task<bool> Update(int id, Product updatedProduct);
        public Task<List<Product>> GetProductsByName(string name);
        public Task<List<Product>> GetProductsByCategory(int id);
        public Task<List<Product>> GetProductsFiltredByPrice(int categoryId,int minPrice, int? maxPrice = null);
        public Task<Product> GetProductWithReviews(int id);
    }
}