using System.Data;
using YonoClothesShop.DTOs;
namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface IInventoryService
    {
        public Task<List<InventoryDTO>> GetProducts();
        public Task<InventoryDTO> GetProduct(int id);
        public Task<List<InventoryDTO>> GetProductsByCategory(int id);
        public Task<List<InventoryDTO>> GetProductsByName(string Name);
        public Task<List<InventoryDTO>> GetProductsFiltredByPrice(int categoryId, int minPrice, int? maxPrice = null);
        public Task<int> AddProduct(string name, string description, IFormFile image, int price, int supplierPrice, int count, int categoryId, string supplierName, string companyName);
        public Task<int> UpdateProduct(int id, string name, string description, IFormFile image, int price, int count);
        public Task<bool> Delete(int id);
    }
}