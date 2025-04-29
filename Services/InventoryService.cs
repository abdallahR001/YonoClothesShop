using YonoClothesShop.DTOs;
using YonoClothesShop.UnitOfWork;
using YonoClothesShop.Models;
using YonoClothesShop.Interfaces.ServicesInterfaces;

namespace YonoClothesShop.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProduct(string name, string description, IFormFile image, int price, int supplierPrice, int count, int categoryId, int supplierId)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || image.Length == 0 || image == null || price <= 0 || count < 0 || categoryId <= 0 || supplierId <= 0)
                return -2;

            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryId);

            if(category == null)
                return 0;

            var supplier = await _unitOfWork.SuppliersRepository.GetById(supplierId);

            if(supplier == null)
                return -3;

            var product = new Inventory
            {
                Name = name,
                Description = description,
                Price = price,
                Count = count,
                CategoryId = categoryId,
                category = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryId),
                SupplierPrice = supplierPrice
            };
            var isAdded = await _unitOfWork.inventoryRepository.AddInventoryProduct(product);

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

            supplier.DeleveriesCount++;

            supplier.TotalDeleveriesPrice += product.Price;

            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }

        public async Task<int> UpdateProduct(int id,string? name, string? description, IFormFile? image, int price, int count)
        {
            var product = await _unitOfWork.inventoryRepository.GetInventoryProductById(id);

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

            var isUpdated = await _unitOfWork.inventoryRepository.UpdateInventoryProduct(product.Id, product);

            if(!isUpdated)
                return -1;

            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var productIsDeleted = await _unitOfWork.inventoryRepository.DeleteInventoryProduct(id);

            if(!productIsDeleted)
                return false;

            await _unitOfWork.SaveChangesAsync();
    
            return true;
        }

        public async Task<List<InventoryDTO>> GetProducts()
        {
            var products = await _unitOfWork.inventoryRepository.GetInventories();

            return products.Select(p => new InventoryDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                SupplierId = p.supplier.Id,
                SupplierName = p.supplier.Name,
                SupplierCompany = p.supplier.CompanyName,
                SupplierPrice = p.SupplierPrice,
            }
            ).ToList();
        }

        public async Task<List<InventoryDTO>> GetProductsByCategory(int id)
        {
            var products = await _unitOfWork.inventoryRepository.GetInventoryProductsByCategory(id);

            if(products == null)
                return null;

            return products.Select(p => new InventoryDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                SupplierId = p.supplier.Id,
                SupplierName = p.supplier.Name,
                SupplierCompany = p.supplier.CompanyName,
                SupplierPrice = p.SupplierPrice,
            }
            ).ToList();
        }

        public async Task<List<InventoryDTO>> GetProductsByName(string name)
        {
            var products = await _unitOfWork.inventoryRepository.GetInventoryProductsByName(name);

            if(!products.Any())
                return null;

            return products.Select(p => new InventoryDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                SupplierId = p.supplier.Id,
                SupplierName = p.supplier.Name,
                SupplierCompany = p.supplier.CompanyName,
                SupplierPrice = p.SupplierPrice,
            }
            ).ToList();
        }

        public async Task<List<InventoryDTO>> GetProductsFiltredByPrice(int categoryId, int minPrice, int? maxPrice = null)
        {
            var products = await _unitOfWork.inventoryRepository.GetInventoryProductsFiltredByPrice(categoryId,minPrice,maxPrice);

            if(!products.Any())
                return null;

            return products.Select(p => new InventoryDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                Price = p.Price,
                Count = p.Count,
                SupplierId = p.supplier.Id,
                SupplierName = p.supplier.Name,
                SupplierCompany = p.supplier.CompanyName,
                SupplierPrice = p.SupplierPrice,
            }
            ).ToList();
        }
    }
}