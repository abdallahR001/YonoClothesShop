using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface ICategoryService
    {
        public Task<int> AddCategory(string name, IFormFile image);
        public Task<bool> UpdateCategory(int id, string? name = null, IFormFile? image = null);
        public Task<bool> DeleteCategory(int id);
        public Task<List<CategoryDTO>> GetCategories();
        public Task<Category> GetById(int id);
        public Task<CategoryDTO> GetCategory(int id);
        public Task<CategoryDTO> GetByName(string name); 
    }
}