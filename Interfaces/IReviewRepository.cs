using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Models;

namespace YonoClothesShop.Interfaces
{
    public interface IReviewRepository
    {
        public Task AddReview(Review review);
        public Task<bool> UpdateReview(int userId,int productId,Review review);
        public Task<bool> DeleteReview(int id);
        public Task<Review> Find(int userId, int productId);
    }
}