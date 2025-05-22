using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface IAuthService
    {
        public Task<Token> Login(string email, string password);
        public Task<Token> LoginAsAdmin(string email, string password);
        public Task<bool> LogOut(int id);
        public Task<Token> RefreshToken(int id, string refreshToken);
    }
}