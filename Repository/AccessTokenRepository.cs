using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class AccessTokenRepository : IbaseRepository<Token>
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Token> Tokens;
        public AccessTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Tokens = _dbContext.Tokens;
        }
        public async Task Add(Token token)
        {
            await _dbContext.Tokens.AddAsync(token);
        }

        public async Task Delete(int id)
        {
            var token = await _dbContext.Tokens.FindAsync(id);
            if(token != null)
                _dbContext.Remove(token);
        }

        public async Task<Token> GetById(int id)
        {
            var token = await _dbContext.Tokens.FindAsync(id);
            return token == null ? null : token;
        }

        public async Task<Token> Update(int id, Token Token)
        {
            var token = await _dbContext.Tokens.FindAsync(id);
            if(token == null)
                return null;
            token.AccessToken = token.AccessToken;
            _dbContext.Tokens.Update(token);
            return token;
        }
    }
}