using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<Order> Orders { get; set; }

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Orders = _dbContext.Orders;
        }

        public async Task<bool> Add(Order order)
        {
            await _dbContext.AddAsync(order);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if(order == null)
                return false;

            _dbContext.Orders.Remove(order);

            return true;
        }
    }
}