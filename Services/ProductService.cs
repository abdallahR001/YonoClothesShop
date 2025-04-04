using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<List<ProductDTO>> GetProductsByCategory(string categoryName)
        {
            var products = await _unitOfWork.ProductsRepository.GetProductsByCategory(categoryName);

            if(products == null)
                return null;

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count
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
                Count = p.Count
            }
            ).ToList();
        }

        public async Task<List<ProductDTO>> GetProductsFiltredByPrice(int minPrice, int? maxPrice = null)
        {
            var products = await _unitOfWork.ProductsRepository.GetProductsFiltredByPrice(minPrice,maxPrice);

            if(!products.Any())
                return null;

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count
            }
            ).ToList();
        }

        public Task<List<ProductDTO>> GetProductsByPriceAndRating(Expression<Func<int, int, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductDTO>> GetProductsByRating(Expression<Func<int, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}