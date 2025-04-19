using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models;
using YonoClothesShop.UnitOfWork;

namespace YonoClothesShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddCategory(string name, IFormFile image)
        {
            if(string.IsNullOrEmpty(name))
                return 0;

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

            var imagePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var category = new Category
            {
                Name = name,
                Image = fileName,
            };

            await _unitOfWork.CategoriesRepository.AddCategory(category);

            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var isDeleted = await _unitOfWork.CategoriesRepository.DeleteCategory(id);

            if(!isDeleted)
                return false;

            return true;
        }

        public Task<Category> GetById(int id)
        {
            var category = _unitOfWork.CategoriesRepository.GetCategoryById(id);

            if(category == null)
                return null;

            return category;
        }

        public Task<Category> GetByName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return null;
            var category = _unitOfWork.CategoriesRepository.GetCategoryByName(name);

            if(category == null)
                return null;

            return category;
        }

        public Task<List<Category>> GetCategories()
        {
            var categories = _unitOfWork.CategoriesRepository.GetCategories();

            return categories;
        }

        public async Task<bool> UpdateCategory(int id, string? name = null, IFormFile? image = null)
        {
            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(id);

            if(category == null)
                return false;

            if(!string.IsNullOrWhiteSpace(name))
                category.Name = name;

            if(image != null && image.Length != 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

                var imagePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                category.Image = fileName;
            }

            return false;
        }
    }
}