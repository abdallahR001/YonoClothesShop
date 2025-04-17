using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ISupplierRepository
    {
        public Task Add(Supplier supplier);
        public Task<bool> Update(int id, string name = null, string companyName = null, string phoneNumber = null);
        public Task<bool> Delete(int id);
        public Task<bool> CheckIfExists(int id);
        public Task<Supplier> GetById(int id);
        public Task<List<Supplier>> GetSuppliers();
        public Task<List<Supplier>> GetSuppliersByDeleveriesCount(int min, int? max = null);
        public Task<List<Supplier>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null);
    }
}