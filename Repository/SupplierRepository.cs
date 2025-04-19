using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.DTOs;
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
            .AnyAsync(s => s.Id == id);

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

        public async Task<List<SupplierDTO>> GetSuppliersByDeleveriesCount(int min, int? max = null)
        {
            var suppliers = _dbContext.Suppliers.Where(s => s.DeleveriesCount >= min);

            if(max.HasValue && max > 0)
                suppliers = suppliers.Where(s => s.DeleveriesCount <= max);

            if(!suppliers.Any())
                return null;

            return await suppliers.Select(s => new SupplierDTO
            {
                Id = s.Id,
                Name = s.Name,
                CompanyName = s.CompanyName,
                PhoneNumber = s.PhoneNumber,
                DeleveriesCount = s.DeleveriesCount,
                TotalDeleveriesPrice = s.TotalDeleveriesPrice
            }
            ).ToListAsync();
        }

        public async Task<List<SupplierDTO>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null)
        {
            var suppliers = _dbContext.Suppliers.Where(s => s.TotalDeleveriesPrice >= min);

            if(max.HasValue && max > 0)
                suppliers = suppliers.Where(s => s.TotalDeleveriesPrice <= max);

            if(!suppliers.Any())
                return null;

            return await suppliers.Select(s => new SupplierDTO
            {
                Id = s.Id,
                Name = s.Name,
                CompanyName = s.CompanyName,
                PhoneNumber = s.PhoneNumber,
                DeleveriesCount = s.DeleveriesCount,
                TotalDeleveriesPrice = s.TotalDeleveriesPrice
            }
            ).ToListAsync();
        }

        public void Update(Supplier supplier)
        {
            _dbContext.Suppliers.Update(supplier);
        }

    }
}