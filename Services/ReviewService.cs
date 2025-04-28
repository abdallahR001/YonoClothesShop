using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.UnitOfWork;
using YonoClothesShop.Models;
namespace YonoClothesShop.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddReview(int userId, int productId, string userReview, int rating)
        {
            var user = await _unitOfWork.UsersRepository.GetById(userId);

            if(user == null)
                return false;

            var product = await _unitOfWork.ProductsRepository.GetById(productId);

            if(product == null)
                return false;

            var isOrderExist = await _unitOfWork.OrdersRepository.CheckIfOrderExist(userId,productId);

            if(isOrderExist)
                return false;

            var review = new Review
            {
                UserId = user.Id,
                ProductId = productId,
                Text = userReview,
                Rate = rating,
                user = user,
            };

            await _unitOfWork.ReviewsRepository.AddReview(review);

            product.reviews.Add(review);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteReview(int userId, int productId)
        {
            var user = await _unitOfWork.UsersRepository.CheckIfUserExsits(userId);

            if(!user)
                return false;

            var product = await _unitOfWork.ProductsRepository.CheckIfProductExsist(productId);

            if(!product)
                return false;
            
            var review = await _unitOfWork.ReviewsRepository.Find(userId,productId);

            if(review == null)
                return false;
            
            await _unitOfWork.ReviewsRepository.DeleteReview(review.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateReview(int userId, int productId, string? userReview = null, int? rating = null)
        {
            if(rating.HasValue && rating < 0)
                return false;

            var user = await _unitOfWork.UsersRepository.CheckIfUserExsits(userId);

            if(!user)
                return false;

            var product = await _unitOfWork.ProductsRepository.CheckIfProductExsist(productId);

            if(!product)
                return false;

            var review = await _unitOfWork.ReviewsRepository.Find(userId,productId);

            if(review == null)
                return false;

            if(!string.IsNullOrWhiteSpace(userReview))
                review.Text = userReview;

            if(rating != null)
                review.Rate = rating;

            await _unitOfWork.ReviewsRepository.UpdateReview(userId,productId,review);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}