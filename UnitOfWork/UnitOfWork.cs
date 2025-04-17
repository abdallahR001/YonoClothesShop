using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Repository;

namespace YonoClothesShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UsersRepository { get; private set; }
        public IProductRepository ProductsRepository { get; private set; }
        public ITokenRepository TokensRepository { get; private set; }
        public ICartRepository CartsRepository { get; private set; }
        public ICartItemRepository CartItemsRepository { get; private set; }
        public IOrderRepository OrdersRepository { get; private set; }
        public IOrderItemRepository OrderItemsRepository { get; private set; }

        public ISupplierRepository SuppliersRepository { get; private set; }

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
            SuppliersRepository = new SupplierRepository(_dbContext);
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
