using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Repository;

namespace YonoClothesShop.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersRepository { get; }
        IProductRepository ProductsRepository { get; }
        ICartRepository CartsRepository { get; }
        ICartItemRepository CartItemsRepository { get; }
        IOrderRepository OrdersRepository { get; }
        IOrderItemRepository OrderItemsRepository { get; }
        ITokenRepository TokensRepository { get; }
        Task<int> SaveChangesAsync();
    }
}