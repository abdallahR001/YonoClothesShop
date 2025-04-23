using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.UnitOfWork;

namespace YonoClothesShop.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProduct(string name, string description, IFormFile image, int price, int count, int categoryId)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || image.Length == 0 || image == null || price <= 0 || count < 0 || categoryId <= 0)
                return -2;

            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryId);

            if(category == null)
                return 0;

            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Count = count,
                CategoryId = categoryId,
                category = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryId),
            };
            var isAdded = await _unitOfWork.ProductsRepository.Add(product);

            if(!isAdded)
                return -1;


            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

            var imagePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            product.Image = fileName;

            category.ProductsCount++;

            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var productIsDeleted = await _unitOfWork.ProductsRepository.Delete(id);

            if(!productIsDeleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
    
            return true;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetById(id);

            if(product == null)
                return null;

            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await _unitOfWork.ProductsRepository.Products.ToListAsync();

            return products;
        }

        public async Task<List<ProductDTO>> GetProductsByCategory(int id)
        {
            var products = await _unitOfWork.ProductsRepository.GetProductsByCategory(id);

            if(products == null)
                return null;

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                reviews = p.reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.user.Name,
                    ProfileImage = r.user.ProfileImage,
                    Text = r.Text,
                    ProductId = r.ProductId,
                    Rate = r.Rate,
                }
                ).ToList()
            }
            ).ToList();
        }

        public async Task<List<ProductDTO>> GetProductsByName(string name)
        {
            var products = await _unitOfWork.ProductsRepository.GetProductsByName(name);

            if(!products.Any())
                return null;

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                reviews = p.reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.user.Name,
                    ProfileImage = r.user.ProfileImage,
                    Text = r.Text,
                    ProductId = r.ProductId,
                    Rate = r.Rate,
                }
                ).ToList()
            }
            ).ToList();
        }

        public async Task<List<ProductDTO>> GetProductsFiltredByPrice(int categoryId, int minPrice, int? maxPrice = null)
        {
            var products = await _unitOfWork.ProductsRepository.GetProductsFiltredByPrice(categoryId,minPrice,maxPrice);

            if(!products.Any())
                return null;

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                reviews = p.reviews != null ? p.reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.user.Name,
                    ProfileImage = r.user.ProfileImage,
                    Text = r.Text,
                    ProductId = r.ProductId,
                    Rate = r.Rate,
                }
                ).ToList() : new List<ReviewDTO>()
            }
            ).ToList();
        }

        public async Task<ProductDTO> GetProductWithReviews(int id)
        {
            var product = await _unitOfWork.ProductsRepository.GetProductWithReviews(id);

            if(product == null)
                return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Count = product.Count,
                Price = product.Price,
                Image = product.Image,
                reviews = product.reviews != null ? product.reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.user.Name,
                    ProfileImage = r.user.ProfileImage,
                    Text = r.Text,
                    ProductId = r.ProductId,
                    Rate = r.Rate,
                }
                ).ToList() : new List<ReviewDTO>()
            };
        }

        public async Task<int> UpdateProduct(int id,string? name, string? description, IFormFile? image, int price, int count)
        {
            var product = await _unitOfWork.ProductsRepository.GetById(id);

            if(product == null)
                return -1;

            if(!string.IsNullOrWhiteSpace(name))
                product.Name = name;

            if(!string.IsNullOrWhiteSpace(description))
                product.Description = description;

            if(image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

                var imagePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                product.Image = $"/images/{fileName}";
            }

            if(price > 0)
                product.Price = price;

            if(count >= 0)
                product.Count = count;

            var isUpdated = await _unitOfWork.ProductsRepository.Update(product.Id, product);

            if(!isUpdated)
                return -1;

            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}