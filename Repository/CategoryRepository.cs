using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddCategory(Category category)
        {
            await _dbContext.AddAsync(category);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if(category == null)
                return false;

            _dbContext.Categories.Remove(category);

            return true;
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            var categories =  await _dbContext.Categories
            .AsNoTracking()
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                ProductsCount = c.ProductsCount,
                Products = c.Products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    Price = p.Price,
                    Count = p.Count
                }
                ).ToList()
            
            })
            .ToListAsync();

            return categories;
        }
        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if(category == null)
                return null;

            return category;
        }

        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            var category = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Name.ToLower() == name.ToLower())
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                ProductsCount = c.ProductsCount,
                Products = c.Products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    Price = p.Price,
                    Count = p.Count
                }
                ).ToList()
            }
            )
            .FirstOrDefaultAsync();

            return category;
        }

        public async Task<bool> UpdateCategory(int id,string name = null, string image = null)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if(category == null)
                return false;

            if(!string.IsNullOrWhiteSpace(name))
                category.Name = name;

            if(!string.IsNullOrWhiteSpace(image))
                category.Image = image;

            return true;
        }
    }
}   