using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IInventoryRepository
    {
        public Task<List<Inventory>> GetInventories();
        public Task<Inventory> GetInventoryProductById(int id);
        public Task<Inventory> GetInventoryProductByName(string name);
        public Task<bool> AddInventoryProduct(Inventory inventory);
        public Task<bool> UpdateInventoryProduct(int id, Inventory inventory);
        public Task<bool> DeleteInventoryProduct(int id);
        public Task<List<Inventory>> GetInventoryProductsByName(string name);
        public Task<List<Inventory>> GetInventoryProductsByCategory(int id);
        public Task<List<Inventory>> GetInventoryProductsFiltredByPrice(int categoryId,int minPrice, int? maxPrice = null);
    }
}