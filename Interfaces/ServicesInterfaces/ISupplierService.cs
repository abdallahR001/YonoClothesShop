using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface ISupplierService
    {
        public Task<bool> Add(string name, string companyName, string phoneNumber);
        public Task<bool> Update(int id,string name = null, string companyName = null, string phoneNumber = null);
        public Task<bool> Delete(int id);
        public Task<SupplierDTO> GetById(int id);
        public Task<List<Supplier>> GetSuppliers();
        public Task<List<SupplierDTO>> GetSuppliersByDeleveriesCount(int min, int? max = null);
        public Task<List<SupplierDTO>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null);
    }
}