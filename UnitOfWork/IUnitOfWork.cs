using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;
using YonoClothesShop.Repository;

namespace YonoClothesShop.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IbaseRepository<User> UsersRepository { get; }
        IbaseRepository<Product> ProductsRepository { get; }
        IbaseRepository<Category> CategoriesRepository { get; }
        IbaseRepository<Cart> CartsRepository { get; }
        IbaseRepository<Order> OrdersRepository { get; }
        IbaseRepository<CartItem> CartItemsRepository { get; }
        IbaseRepository<OrderItem> OrderItemsRepository { get; }
        IbaseRepository<Review> ReviewsRepository { get; }
        IbaseRepository<Supplier> SuppliersRepository { get; }
        IbaseRepository<Inventory> InventoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}