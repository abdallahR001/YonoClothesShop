using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ISupplierRepository
    {
        public Task Add(Supplier supplier);
        public void Update(Supplier supplier);
        public Task<bool> Delete(int id);
        public Task<bool> CheckIfExists(int id);
        public Task<Supplier> GetById(int id);
        public Task<List<Supplier>> GetSuppliers();
        public Task<List<SupplierDTO>> GetSuppliersByDeleveriesCount(int min, int? max = null);
        public Task<List<SupplierDTO>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null);
    }
}