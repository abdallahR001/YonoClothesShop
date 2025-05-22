using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.Data;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public AdminsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var usersCount = _dbContext.Users.Count();

            var ordersCount = _dbContext.Orders.Count();

            var totalProfit = _dbContext.Orders.Sum(o => o.TotalPrice);

            var suppliersCount = _dbContext.Suppliers.Count();

            var productsCount = _dbContext.Products.Count();

            var categoriesCount = _dbContext.Categories.Count();

            return Ok(new {
                UsersCount = usersCount,

                OrdersCount = ordersCount,

                TotalProfit = totalProfit,

                ProductsCount = productsCount,

                CategoriesCount = categoriesCount,

                SuppliersCount = suppliersCount
            });
        }
    }
}