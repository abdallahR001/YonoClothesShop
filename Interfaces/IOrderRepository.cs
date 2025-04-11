using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IOrderRepository
    {
        public IQueryable<Order> Orders { get; set; }
        public Task<bool> Add(Order orderItem);
        public Task<bool> Delete(int id);
    }
}