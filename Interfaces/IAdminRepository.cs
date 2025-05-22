using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IAdminRepository
    {
        public Task<Admin> GetByEmail(string email);
    }
}