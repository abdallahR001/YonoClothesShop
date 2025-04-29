using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using Microsoft.EntityFrameworkCore;
namespace YonoClothesShop.Repository
{
    public class InventoryProductRepository : IInventoryRepository
    {
        private readonly AppDbContext _dbContext;

        public InventoryProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Inventory>> GetInventories()
        {
            var inventoryProducts = await _dbContext.InventoryProducts
            .AsNoTracking()
            .Include(p => p.supplier)
            .ToListAsync();

            return inventoryProducts;
        }
        public async Task<bool> AddInventoryProduct(Inventory inventory)
        {
            var exsistingInventoryProduct = await _dbContext.InventoryProducts.FirstOrDefaultAsync(p => p.Name == inventory.Name);

            if(exsistingInventoryProduct == null)
            {
                await _dbContext.InventoryProducts.AddAsync(inventory);

                return true;
            }
                
            return false;
        }

        public async Task<bool> DeleteInventoryProduct(int id)
        {
            var inventoryProduct = await _dbContext.InventoryProducts.FindAsync(id);

            if(inventoryProduct != null)
            {
                _dbContext.InventoryProducts.Remove(inventoryProduct);

                return true;
            }
            
            return false;
        }

        public async Task<Inventory> GetInventoryProductById(int id)
        {
            var inventoryProduct = await _dbContext.InventoryProducts.FindAsync(id);

            if(inventoryProduct == null)
                return null;
            
            return inventoryProduct;
        }

        public async Task<bool> UpdateInventoryProduct(int id, Inventory updatedInventoryProduct)
        {
            var inventoryProduct = await _dbContext.InventoryProducts.FindAsync(id);

            if(inventoryProduct == null)
                return false;

            if(!string.IsNullOrWhiteSpace(updatedInventoryProduct.Name))
                inventoryProduct.Name = updatedInventoryProduct.Name;

            if(!string.IsNullOrWhiteSpace(updatedInventoryProduct.Description))
                inventoryProduct.Description = updatedInventoryProduct.Description;

            if(!string.IsNullOrWhiteSpace(updatedInventoryProduct.Image))
                inventoryProduct.Image = updatedInventoryProduct.Image;

            if(updatedInventoryProduct.Price > 0)
                inventoryProduct.Price = updatedInventoryProduct.Price;

            if(updatedInventoryProduct.Count >= 0)
                inventoryProduct.Count = updatedInventoryProduct.Count;
            
            return true;
        }
        public async Task<List<Inventory>> GetInventoryProductsByName(string name)
        {
            var InventoryProducts = await _dbContext.InventoryProducts
            .Include(p => p.supplier)
            .Where(p => p.Name.Contains(name))
            .ToListAsync();

            if(!InventoryProducts.Any())
                return null;
            
            return InventoryProducts;
        }
        public async Task<List<Inventory>> GetInventoryProductsByCategory(int id)
        {
            var category = await _dbContext.Categories
            .FindAsync(id);

            if(category == null)
                return null;

            var InventoryProducts = await _dbContext.InventoryProducts
            .Where(p => p.CategoryId == category.Id)
            .ToListAsync();

            return InventoryProducts;
        }
        public async Task<List<Inventory>> GetInventoryProductsFiltredByPrice(int categoryId,int minPrice, int? maxPrice = null)
        {
            if(minPrice <= 0)
                return null;

            var filteredInventoryProducts = _dbContext.InventoryProducts.Where(p => p.CategoryId == categoryId && p.Price >= minPrice);

            if(maxPrice.HasValue && maxPrice > 0)
                filteredInventoryProducts = filteredInventoryProducts.Where(p => p.Price <= maxPrice);

            return await filteredInventoryProducts.ToListAsync();
        }
    }
}