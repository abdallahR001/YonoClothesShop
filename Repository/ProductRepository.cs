using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Product> Products { get; set; }

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Products = _dbContext.Products;
        }
        public async Task<bool> Add(Product product)
        {
            var exsistingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == product.Name);

            if(exsistingProduct == null)
            {
                await _dbContext.AddAsync(product);
                return true;
            }
                
            else
                return false;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product != null)
            {
                _dbContext.Products.Remove(product);
                return true;
            }
            
            return false;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null)
                return null;
            
            return product;
        }

        public async Task<bool> Update(int id, Product updatedProduct)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null)
                return false;

            if(!string.IsNullOrWhiteSpace(updatedProduct.Name))
                product.Name = updatedProduct.Name;

            if(!string.IsNullOrWhiteSpace(updatedProduct.Description))
                product.Description = updatedProduct.Description;

            if(!string.IsNullOrWhiteSpace(updatedProduct.Image))
                product.Image = updatedProduct.Image;

            if(updatedProduct.Price > 0)
                product.Price = updatedProduct.Price;

            if(updatedProduct.Count >= 0)
                product.Count = updatedProduct.Count;
            
            return true;
        }
        public async Task<List<Product>> GetProductsByName(string name)
        {
            var products = await _dbContext.Products
            .Include(r => r.reviews)
            .ThenInclude(u => u.user)
            .Where(p => p.Name.Contains(name))
            .ToListAsync();

            if(!products.Any())
                return null;
            
            return products;
        }
        public async Task<List<Product>> GetProductsByCategory(int id)
        {
            var category = await _dbContext.Categories
            .FindAsync(id);

            if(category == null)
                return null;

            var products = await _dbContext.Products
            .Where(p => p.CategoryId == category.Id)
            .ToListAsync();

            return products;
        }
        public async Task<List<Product>> GetProductsFiltredByPrice(int categoryId,int minPrice, int? maxPrice = null)
        {
            if(minPrice <= 0)
                return null;

            var filtredProducts = _dbContext.Products.Where(p => p.CategoryId == categoryId && p.Price >= minPrice);

            if(maxPrice.HasValue && maxPrice > 0)
                filtredProducts = filtredProducts.Where(p => p.Price <= maxPrice);

            return await filtredProducts.ToListAsync();
        }

        public async Task<bool> CheckIfProductExsist(int id)
        {
            var product = await _dbContext.Products.AnyAsync(p => p.Id == id);

            if(!product)
                return false;

            return true;
        }

        public async Task<Product> GetProductWithReviews(int id)
        {
            var product = await _dbContext.Products
            .Include(r => r.reviews)
            .ThenInclude(r => r.user)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

            return product;
        }
    }
}