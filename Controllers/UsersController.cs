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
        public async Task<ActionResult<string>> Login(LoginModel request)
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
}
}