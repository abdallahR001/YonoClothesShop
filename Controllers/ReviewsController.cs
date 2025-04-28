using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YonoClothesShop.Models.RequestModels;
namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        public IReviewService _ReviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _ReviewService = reviewService;
        }
        [HttpPost("{productId}"),Authorize]
        public async Task<ActionResult> AddReview(int productId, AddReviewModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isAdded = await _ReviewService.AddReview(id,productId,request.Review,request.Rating);

            if(!isAdded)
                return NotFound(new {message = "user or product not found"});
            
            return Ok(new {message = "added review successfully"});
        }
        [HttpPut("{productId}"),Authorize]
        public async Task<ActionResult> UpdateTask(int productId, UpdateReviewModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();
            
            var isUpdated = await _ReviewService.UpdateReview(id,productId,request.Review,request.Rating);

            if(!isUpdated)
                return NotFound(new {message = "user or product or review not found"});

            return Ok(new {message = "updated successfully"});
        }
        [HttpDelete("{productId}"),Authorize]
        public async Task<ActionResult> DeleteReview(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isDeleted = await _ReviewService.DeleteReview(id,productId);  

            if(!isDeleted)
                return NotFound(new {message = "user or product or review not found"});

            return Ok(new {message = "deleted successfully"});
        }
    }
}