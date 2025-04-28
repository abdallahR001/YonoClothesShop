using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface IReviewService
    {
        public Task<bool> AddReview(int userId,int productId,string userReview, int rating);
        public Task<bool> UpdateReview(int userId,int productId,string? userReview = null, int? rating = null);
        public Task<bool> DeleteReview(int userId,int productId);
    }
}