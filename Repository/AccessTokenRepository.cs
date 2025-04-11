using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class AccessTokenRepository : ITokenRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Token> Tokens { get; set; }
        public AccessTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Tokens = _dbContext.Tokens;
        }
        public async Task<bool> Add(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var token = await _dbContext.Tokens.FindAsync(id);
            if(token != null)
            {
                _dbContext.Remove(token);

                return true;
            }

            return false;
        }

        public Task<Token> Find(string token)
        {
            var Token = _dbContext.Tokens
            .FirstOrDefaultAsync(t => t.AccessToken == token);

            if(token == null)
                return null;

            return Token;
        }
    }
}