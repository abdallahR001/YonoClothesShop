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
        public Task<UserDTO> GetAccount(int id);
        public Task<bool> UpdateAccount(int id, string name = null, string address = null, IFormFile profileImage = null);
        public Task<bool> DeleteAccount(int id);
        public Task<List<OrderDTO>> GetOrders(int id);
        public Task<bool> Deposit(int id, int amount);
        
    }
}