using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Models.RequestModels;
using YonoClothesShop.TokenGenerator;
using YonoClothesShop.UnitOfWork;

namespace YonoClothesShop.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAccount(string name, string email, string password, string address, IFormFile profileImage)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address) || profileImage == null)
                return false;
            var userExists = await _unitOfWork.UsersRepository.GetByEmail(email);
            if(userExists != null)
                return false;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }
            var imagePath = Path.Combine("images", fileName);
            
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Address = address,
                ProfileImage = fileName
            };
            await _unitOfWork.UsersRepository.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAccount(int id)
        {
            var userDeleted = await _unitOfWork.UsersRepository.Delete(id);

            if(!userDeleted)
                return false;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }


        public async Task<bool> Deposit(int id, int amount)
        {

            if(amount <= 0)
                return false;

            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            user.Amount += amount;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<UserDTO> GetAccount(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return null;
            
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Amount = user.Amount,
                Address = user.Address,
                OrdersCount = user.OrdersCount,
                ProfileImage = user.ProfileImage
            };
        }

        public async Task<bool> UpdateAccount(int id,string name = null, string address = null, IFormFile profileImage = null)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            if(!string.IsNullOrWhiteSpace(name))
                user.Name = name;

            if(!string.IsNullOrWhiteSpace(address))
                user.Address = address;

            if(profileImage != null && profileImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profileImage.FileName)}";

                var imagePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                user.ProfileImage = $"/images/{fileName}";
            }

            await _unitOfWork.UsersRepository.Update(id,user);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}