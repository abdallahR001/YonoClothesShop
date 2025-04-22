using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using YonoClothesShop.DTOs;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateAccount(string name, string email, string password, string address, IFormFile profileImage);
        public Task<Token> Login(string email, string password);
        public Task<bool> LogOut(int id);
        public Task<Token> RefreshToken(int id, string refreshToken);
        public Task<UserDTO> GetAccount(int id);
        public Task<bool> UpdateAccount(int id, string name = null, string address = null, IFormFile profileImage = null);
        public Task<bool> DeleteAccount(int id);
        public Task<bool> AddReview(int userId,int productId,string userReview, int rating);
        public Task<bool> AddProductToCart(int userId,int productId, int quantity);
        public Task<int> RemoveProductFromCart(int userId,int productId);
        public Task<CartDTO> ViewCart(int id);
        public Task<bool> ClearCart(int id);
        public Task<bool> Deposit(int id, int amount);
        public Task<bool> Checkout(int id);
    }
}