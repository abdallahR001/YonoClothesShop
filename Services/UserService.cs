using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
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
        public Task<bool> AddProductToCart(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewDTO> AddReview(string review, int rating)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Checkout(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCart()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateAccount(string name, string email, string password, string address, IFormFile profileImage)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address) || profileImage == null)
                return false;
            var userExists = await _unitOfWork.UsersRepository.GetByEmail(email);
            if(userExists)
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
                profileImage.CopyTo(stream);
            }
            var imagePath = Path.Combine("images", fileName);
            
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Address = address,
                ProfileImage = imagePath
            };
            await _unitOfWork.UsersRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveProductFromCart(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccount(string name = null, string email = null, string password = null, string address = null, IFormFile profileImage = null)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> ViewCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}