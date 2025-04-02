using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Data;
using YonoClothesShop.Models;
using YonoClothesShop.Repository;

namespace YonoClothesShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UserRepository UsersRepository { get; private set; }
        public AccessTokenRepository TokenRepository { get; private set; }
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            UsersRepository = new UserRepository(_dbContext);
            TokenRepository = new AccessTokenRepository(_dbContext);
        }
        

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
             _dbContext.Dispose();
        }
    }

    }
