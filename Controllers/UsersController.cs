using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Models.RequestModels;
using YonoClothesShop.Services;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest("invalid data");
            var result = await _userService.CreateAccount(request.Name, request.Email, request.Password, request.Address, request.ProfileImage);
            if(result)
                return Ok(new {message="account created succesfully"});
            return BadRequest(new {message = "user already exists"});
        }
        [HttpPost("login")]
        public async Task<ActionResult<Token>> Login(LoginModel request)
        {
            var token = await _userService.Login(request.Email, request.Password);
            if(token == null)
                return BadRequest(new {message = "invalid credentials"});
            return Ok(token);
        }
        [HttpGet("logout"),Authorize]
        public async Task<ActionResult> LogOut()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Unauthorized();
            
            var isLoggedOut = await _userService.LogOut(id);

            if(isLoggedOut)
                return Ok(new {message = "logged out successfully"});

            return NotFound(new {message = "user or token not found"});
        }
        [HttpGet("refresh-token"),Authorize]
        public async Task<ActionResult> RefreshToken([FromHeader] string refreshToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Unauthorized();

            var newToken = await _userService.RefreshToken(id,refreshToken);

            if(newToken == null)
                return NotFound(new {message = "user or token not found"});

            return Ok(newToken);
        }
        [HttpGet("profile"),Authorize]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var user = await _userService.GetAccount(id);

            if(user == null)
                return NotFound(new {message = "user not found"});
            
            return Ok(user);
        }
        [HttpPut("update-profile"),Authorize]
        public async Task<ActionResult> UpdateAccount(UpdateUserProfileModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isUpdated = await _userService.UpdateAccount(id,request.Name,request.Address,request.ProfileImage);

            if(!isUpdated)
                return NotFound(new {message = "user not found"});

            return Ok(new {message = "updated successfully"});
        }
        [HttpDelete("delete-account"),Authorize]
        public async Task<ActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isDeleted = await _userService.DeleteAccount(id);

            if(!isDeleted)
                return NotFound(new {message = "user not found"});

            return Ok(new {message = "account deleted successfully"});
        }
        [HttpPost("{productId}/add-review"),Authorize]
        public async Task<ActionResult> AddReview(int productId, AddReviewModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isAdded = await _userService.AddReview(id,productId,request.Review,request.Rating);

            if(!isAdded)
                return NotFound(new {message = "user or product not found"});
            
            return Ok(new {message = "added review successfully"});
        }
        [HttpPut("{productId}/update-review"),Authorize]
        public async Task<ActionResult> UpdateTask(int productId, UpdateReviewModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();
            
            var isUpdated = await _userService.UpdateReview(id,productId,request.Review,request.Rating);

            if(!isUpdated)
                return NotFound(new {message = "user or product or review not found"});

            return Ok(new {message = "updated successfully"});
        }
        [HttpDelete("{productId}/delete-review"),Authorize]
        public async Task<ActionResult> DeleteReview(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var isDeleted = await _userService.DeleteReview(id,productId);  

            if(!isDeleted)
                return NotFound(new {message = "user or product or review not found"});

            return Ok(new {message = "deleted successfully"});
        }
        [HttpPost("deposit"),Authorize]
        public async Task<ActionResult> Deposit(DepositModel request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Forbid();

            var result = await _userService.Deposit(id,request.Amount);

            if(!result)
                return BadRequest(new {message = "user not found or amount is less than or equal to 0"});

            return Ok(new {messge = "amount added successfully"});
        }
        [HttpPost("add-to-cart/{productId}"),Authorize]
        public async Task<ActionResult> AddToCart(int productId, AddProductToCartModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "bad data"});
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isAddedToCart = await _userService.AddProductToCart(id,productId,request.Quantity);

            if(!isAddedToCart)
                return NotFound(new {message = "failed to add to cart because user or cart was not found"});

            return Ok(new {message = "added successfully"});
        }
        [HttpPut("remove-from-cart/{productId}"),Authorize]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isRemoved = await _userService.RemoveProductFromCart(id,productId);

            if(isRemoved == 0)
                return NotFound(new {message = "cart not found"});

            if(isRemoved == -1)
                return NotFound(new {message = "cart is empty"});

            if(isRemoved == -2)
                return NotFound(new {message = "product not found"});

            return Ok(new {message = "removed successfully"});
        }
        [HttpGet("view-cart"),Authorize]
        public async Task<ActionResult<CartDTO>> ViewCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var cart = await _userService.ViewCart(id);

            if(cart == null)
                return NotFound(new {message = "cart not found"});

            return Ok(cart);
        }
        [HttpPut("clear-cart"),Authorize]
        public async Task<ActionResult> ClearCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isCleared = await _userService.ClearCart(id);

            if(!isCleared)
                return NotFound(new {message = "cart not found"});

            return Ok(new {message = "deleted successfully"});
        }
        [HttpGet("checkout"),Authorize]
        public async Task<ActionResult> Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var result = await _userService.Checkout(id);

            if(!result)
                return NotFound(new {message = "user or cart or cart items not found"});
            return Ok("placed order successfully");
        }
}
}