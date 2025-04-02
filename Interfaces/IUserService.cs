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
        public Task<UserDTO> GetAccount(int id);
        public Task<bool> UpdateAccount(string name = null, string email = null, string password = null, string address = null, IFormFile profileImage = null);
        public Task<bool> DeleteAccount(int id);
        public Task<ReviewDTO> AddReview(string review, int rating);
        public Task<bool> AddProductToCart(int productId, int quantity);
        public Task<bool> RemoveProductFromCart(int productId);
        public Task<Cart> ViewCart(int id);
        public Task<bool> ClearCart();
        public Task<bool> Checkout(int id);
    }
}