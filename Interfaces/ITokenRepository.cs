using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface ITokenRepository
    {
        public IQueryable<Token> Tokens { get; set; }
        public Task<bool> Add(Token token);
        public Task<bool> Delete(int id);
        public Task<Token> Find(string token);
    }
}