using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class OrderItemRepository : IbaseRepository<OrderItem>
    {
        private readonly AppDbContext _dbContext;
        IQueryable<OrderItem> OrderItems;

        public OrderItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            OrderItems = _dbContext.OrderItems;
        }
        public async Task<bool> Add(OrderItem orderItem)
        {
            var existingOrderItem = await _dbContext.OrderItems
            .FirstOrDefaultAsync(o => o.ProductId == orderItem.ProductId && o.OrderId == orderItem.OrderId);

            if(existingOrderItem != null)
            {
                existingOrderItem.Quantity += orderItem.Quantity;

                return true;
            }

            await _dbContext.AddAsync(orderItem);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);

            if(orderItem == null)
                return false;

            _dbContext.OrderItems.Remove(orderItem);

            return true;
        }

        public async Task<OrderItem> GetById(int id)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(id);

            if(orderItem == null)
                return null;

            return orderItem;
        }

        public Task<bool> Update(int id, OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }
}