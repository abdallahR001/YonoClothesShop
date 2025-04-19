using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ICategoryRepository
    {
        public Task AddCategory(Category category);
        public Task<bool> UpdateCategory(int id, string name, string image);
        public Task<bool> DeleteCategory(int id);
        public Task<List<Category>> GetCategories();
        public Task<Category> GetCategoryById(int id);
        public Task<Category> GetCategoryByName(string name);
    }
}