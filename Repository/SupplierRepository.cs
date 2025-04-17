using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _dbContext;

        public SupplierRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Supplier supplier)
        {
            await _dbContext.Suppliers.AddAsync(supplier);
        }

        public async Task<bool> CheckIfExists(int id)
        {
            var supplier = await _dbContext.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

            if(supplier == null)
                return false;

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if(supplier == null)
                return false;

            _dbContext.Remove(supplier);

            return true;
        }

        public async Task<Supplier> GetById(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            return supplier != null ? supplier : null;
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            return await _dbContext.Suppliers.ToListAsync();
        }

        public async Task<List<Supplier>> GetSuppliersByDeleveriesCount(int min, int? max = null)
        {
            var suppliers = _dbContext.Suppliers.Where(s => s.DeleveriesCount >= min);

            if(max.HasValue && max > 0)
                suppliers = _dbContext.Suppliers.Where(s => s.DeleveriesCount <= max);

            if(!suppliers.Any())
                return null;

            return await suppliers.ToListAsync();
        }

        public async Task<List<Supplier>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null)
        {
            var suppliers = _dbContext.Suppliers.Where(s => s.TotalDeleveriesPrice >= min);

            if(max.HasValue && max > 0)
                suppliers = suppliers.Where(s => s.TotalDeleveriesPrice <= max);

            if(!suppliers.Any())
                return null;

            return await suppliers.ToListAsync();
        }

        public async Task<bool> Update(int id,string name = null, string companyName = null, string phoneNumber = null)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if(supplier == null)
                return false;

            if(!string.IsNullOrWhiteSpace(name))
                supplier.Name = name;

            if(!string.IsNullOrWhiteSpace(companyName))
                supplier.CompanyName = companyName;

            if(!string.IsNullOrWhiteSpace(phoneNumber))
                supplier.PhoneNumber = phoneNumber;

            return true;
        }
    }
}