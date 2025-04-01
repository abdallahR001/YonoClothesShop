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
        public IbaseRepository<User>? UsersRepository { get; private set; }
        public IbaseRepository<Product>? ProductsRepository { get; private set; }
        public IbaseRepository<Category>? CategoriesRepository { get; private set; }
        public IbaseRepository<Cart>? CartsRepository { get; private set; }
        public IbaseRepository<Order>? OrdersRepository { get; private set; }
        public IbaseRepository<CartItem>? CartItemsRepository { get; private set; }
        public IbaseRepository<OrderItem>? OrderItemsRepository { get; private set; }
        public IbaseRepository<Review>? ReviewsRepository { get; private set; }
        public IbaseRepository<Supplier>? SuppliersRepository { get; private set; }
        public IbaseRepository<Inventory>? InventoryRepository { get; private set; }
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            UsersRepository = new BaseRepository<User>(dbContext);
            ProductsRepository = new BaseRepository<Product>(dbContext);
            CategoriesRepository = new BaseRepository<Category>(dbContext);
            CartsRepository = new BaseRepository<Cart>(dbContext);
            OrdersRepository = new BaseRepository<Order>(dbContext);
            CartItemsRepository = new BaseRepository<CartItem>(dbContext);
            OrderItemsRepository = new BaseRepository<OrderItem>(dbContext);
            ReviewsRepository = new BaseRepository<Review>(dbContext);
            SuppliersRepository = new BaseRepository<Supplier>(dbContext);
            InventoryRepository = new BaseRepository<Inventory>(dbContext);
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