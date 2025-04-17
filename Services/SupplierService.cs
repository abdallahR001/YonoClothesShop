using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models;
using YonoClothesShop.UnitOfWork;

namespace YonoClothesShop.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Add(string name, string companyName, string phoneNumber)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var supplier = new Supplier
            {
                Name = name,
                CompanyName = companyName,
                PhoneNumber = phoneNumber,
            };

            await _unitOfWork.SuppliersRepository.Add(supplier);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var supplier = await _unitOfWork.SuppliersRepository.CheckIfExists(id);

            if(!supplier)
                return false;

            _unitOfWork.SuppliersRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<SupplierDTO> GetById(int id)
        {
            var supplier = await _unitOfWork.SuppliersRepository.GetById(id);

            if(supplier == null)
                return null;

            return new SupplierDTO
            {
                Name = supplier.Name,
                CompanyName = supplier.CompanyName,
                PhoneNumber = supplier.PhoneNumber,
                DeleveriesCount = supplier.DeleveriesCount,
                TotalDeleveriesPrice = supplier.TotalDeleveriesPrice,
            };
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            return await _unitOfWork.SuppliersRepository.GetSuppliers();
        }

        public Task<List<SupplierDTO>> GetSuppliersByDeleveriesCount(int min, int? max = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupplierDTO>> GetSuppliersByTotalDeleveriesPrice(int min, int? max = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, string name = null, string companyName = null, string phoneNumber = null)
        {
            throw new NotImplementedException();
        }
    }
}