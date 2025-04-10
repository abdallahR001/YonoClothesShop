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
        public ProductRepository ProductsRepository { get; private set; }
        public AccessTokenRepository TokensRepository { get; private set; }

        public CartRepository CartsRepository { get; private set; }

        public CartItemsRepository CartItemsRepository { get; private set; }

        public OrderRepository OrdersRepository { get; private set; }

        public OrderItemRepository OrderItemsRepository { get; private set; }

        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            UsersRepository = new UserRepository(_dbContext);
            ProductsRepository = new ProductRepository(_dbContext);
            CartsRepository = new CartRepository(_dbContext);
            CartItemsRepository = new CartItemsRepository(_dbContext);
            OrdersRepository = new OrderRepository(_dbContext);
            OrderItemsRepository = new OrderItemRepository(_dbContext);
            TokensRepository = new AccessTokenRepository(_dbContext);
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
