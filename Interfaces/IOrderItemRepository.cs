using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IOrderItemRepository
    {
        public IQueryable<OrderItem> OrderItems { get; set; }
        public Task<bool> Add(OrderItem orderItem);
        public Task<bool> Delete(int id);
    }
}