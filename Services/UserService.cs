using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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
        public async Task<Token> Login(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _unitOfWork.UsersRepository.GetByEmail(email);
            if(user == null)
                return null;

            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password,user.PasswordHash);
            if(!isPasswordCorrect)
                return null;

            var accessToken = UserTokenGenerator.GenerateToken(user.Id,email,_configuration);
            var refreshToken = UserTokenGenerator.GenerateRefreshToken();

            var existingToken = await _unitOfWork.TokenRepository.Tokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
            if(existingToken != null)
            {
                existingToken.AccessToken = accessToken;

                existingToken.RefreshToken = refreshToken;

                existingToken.AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

                existingToken.RefreshTokenExpiration = DateTime.UtcNow
                .AddDays(5);

                await _unitOfWork.SaveChangesAsync();

                return existingToken;
            }
            var authResponse = new Token
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(5)
            };

            _unitOfWork.TokenRepository.Add(authResponse);

            await _unitOfWork.SaveChangesAsync();

            return authResponse;
        }

        public async Task<bool> LogOut(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            var token = await _unitOfWork.TokenRepository.Tokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if(token == null || token.RefreshTokenExpiration < DateTime.UtcNow)
                return false;
            
            await _unitOfWork.TokenRepository.Delete(token.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
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