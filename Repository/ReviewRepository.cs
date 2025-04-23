using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Data;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;

namespace YonoClothesShop.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;

        public ReviewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddReview(Review review)
        {
            await _dbContext.AddAsync(review);
        }

        public async Task<bool> DeleteReview(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);

            if(review == null)
                return false;

            _dbContext.Remove(review);

            return true;
        }

        public async Task<Review> Find(int userId, int productId)
        {
            var review = await _dbContext.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if(review == null)
                return null;

            return review;
        }

        public async Task<bool> UpdateReview(int userId, int productId, Review review)
        {
            var user = await _dbContext.Users.AnyAsync(u => u.Id == userId);

            if(!user)
                return false;

            var product = await _dbContext.Products.AnyAsync(p => p.Id == productId);

            if(!product)
                return false;

            var userReview = await _dbContext.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if(userReview == null)
                return false;

            userReview.Text = review.Text;

            userReview.Rate = review.Rate;

            return true;
        }
    }
}