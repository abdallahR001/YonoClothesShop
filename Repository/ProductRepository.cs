using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class ProductRepository : IbaseRepository<Product>
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Product> Products;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Products = _dbContext.Products;
        }
        public async Task Add(Product product)
        {
            await _dbContext.AddAsync(product);
        }

        public async Task Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product != null)
            {
                _dbContext.Products.Remove(product);
            }
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null)
                return null;
            
            return product;
        }

        public async Task<Product> Update(int id, Product updatedProduct)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null)
                return null;

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Image = updatedProduct.Image;
            product.Price = updatedProduct.Price;
            product.Count = updatedProduct.Count;
            
            return product;
        }
        public async Task<List<Product>> GetProductsByName(string name)
        {
            var products = await _dbContext.Products
            .Where(p => p.Name.Contains(name))
            .ToListAsync();

            if(!products.Any())
                return null;
            
            return products;
        }
        public async Task<List<Product>> GetProductsByCategory(string categoryName)
        {
            var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == categoryName);

            if(category == null)
                return null;

            var products = await _dbContext.Products
            .Where(p => p.category.Name == categoryName)
            .ToListAsync();

            return products;
        }
        public async Task<List<Product>> GetProductsFiltredByPrice(int minPrice, int? maxPrice = null)
        {
            if(minPrice <= 0)
                return null;

            var filtredProducts = Products.Where(p => p.Price >= minPrice);

            if(maxPrice.HasValue && maxPrice > 0)
                filtredProducts = filtredProducts.Where(p => p.Price <= maxPrice);

            return await filtredProducts.ToListAsync();
        }
    }
}