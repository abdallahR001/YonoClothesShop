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
        UserRepository UsersRepository { get; }
        ProductRepository ProductsRepository { get; }
        AccessTokenRepository TokensRepository { get; }
        Task<int> SaveChangesAsync();
    }
}