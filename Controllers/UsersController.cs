using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
}
}