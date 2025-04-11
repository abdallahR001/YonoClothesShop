using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _dbContext;
        public IQueryable<OrderItem> OrderItems { get; set; }
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
    }
}