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
    public class AdminRepostiory : IAdminRepository
    {
        public AppDbContext _dbContext;

        public AdminRepostiory(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public async Task<Admin> GetByEmail(string email)
        {
            var admin = await _dbContext.Admins
            .FirstOrDefaultAsync(a => a.Email == email);

            if(admin == null)
                return null;

            return admin;
        }
    }
}