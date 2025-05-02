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

        public async Task<int> AddProduct(string name, string description, IFormFile image, int price, int supplierPrice, int count, int categoryId,string supplierName,string companyName)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || image.Length == 0 || image == null || price <= 0 || count < 0 || categoryId <= 0 || string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(companyName))
                return -2;

            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryId);

            if(category == null)
                return 0;

            var supplier = await _unitOfWork.SuppliersRepository.GetSupplierByNameAndCompanyName(supplierName,companyName);

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
                supplier = supplier,
                SupplierId = supplier.Id,
                SupplierPrice = supplierPrice,
                AddedAt = DateTime.UtcNow,
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

        public async Task<InventoryDTO> GetProduct(int id)
        {
            var product = await _unitOfWork.inventoryRepository.GetInventoryProductById(id);

            if(product == null)
                return null;

            return new InventoryDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Count = product.Count,
                CategoryId = product.CategoryId,
                SupplierId = product.supplier.Id,
                SupplierName = product.supplier.Name,
                SupplierCompany = product.supplier.CompanyName,
                SupplierPrice = product.SupplierPrice,
            };
        }

        public async Task<int> UpdateProduct(int id,string? name, string? description, IFormFile? image, int price, int count)
        {
            var inventoryProduct = await _unitOfWork.inventoryRepository.GetInventoryProductById(id);

            if(inventoryProduct == null)
                return -1;

            var product = await _unitOfWork.ProductsRepository.GetProductByName(inventoryProduct.Name);

            if(!string.IsNullOrWhiteSpace(name))
            {
                inventoryProduct.Name = name;

                if(product != null)
                    product.Name = name;
            }

            if(!string.IsNullOrWhiteSpace(description))
            {
                inventoryProduct.Description = description;

                if(product != null)
                    product.Description = description;
            }

            if(image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";

                var imagePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                inventoryProduct.Image = $"/images/{fileName}";

                if(product != null)
                    product.Image = $"/images/{fileName}";
            }

            if(price > 0)
            {
                inventoryProduct.Price = price;
                if(product != null)
                    product.Price = price;
            }
                
            if(count >= 0)
                inventoryProduct.Count = count;

            var isUpdated = await _unitOfWork.inventoryRepository.UpdateInventoryProduct(inventoryProduct.Id, inventoryProduct);

            if(!isUpdated)
                return -1;

            await _unitOfWork.SaveChangesAsync();

            return inventoryProduct.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var inventoryProduct = await _unitOfWork.inventoryRepository.GetInventoryProductById(id);

            if(inventoryProduct == null)
                return false;

            var product = await _unitOfWork.ProductsRepository.GetProductByName(inventoryProduct.Name);

            var productIsDeleted = await _unitOfWork.inventoryRepository.DeleteInventoryProduct(inventoryProduct.Id);

            if(!productIsDeleted)
                return false;

            if(product != null)
                await _unitOfWork.ProductsRepository.Delete(product.Id);

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
                CategoryId = p.CategoryId,
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
                CategoryId = p.CategoryId,
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
                CategoryId = p.CategoryId,
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
                CategoryId = p.CategoryId,
                SupplierId = p.supplier.Id,
                SupplierName = p.supplier.Name,
                SupplierCompany = p.supplier.CompanyName,
                SupplierPrice = p.SupplierPrice,
            }
            ).ToList();
        }
    }
}