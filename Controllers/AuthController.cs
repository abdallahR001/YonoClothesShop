using YonoClothesShop.Models;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models.RequestModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("")]
        public async Task<ActionResult<Token>> Login(LoginModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid credentials"});
            var token = await _authService.Login(request.Email, request.Password);
            if(token == null)
                return BadRequest(new {message = "invalid credentials"});
            return Ok(token);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<Token>> LoginAsAdmin(LoginModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid credintials"});

            var token = await _authService.LoginAsAdmin(request.Email,request.Password);

            if(token == null)
                return BadRequest(new {message = "invalid credintials"});

            return Ok(token);
        }

        [HttpDelete(""),Authorize]
        public async Task<ActionResult> LogOut()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Unauthorized();
            
            var isLoggedOut = await _authService.LogOut(id);

            if(isLoggedOut)
                return Ok(new {message = "logged out successfully"});

            return NotFound(new {message = "user or token not found"});
        }

        [HttpGet("refresh-token"),Authorize(Roles = "admin,user")]
        public async Task<ActionResult> RefreshToken([FromHeader] string refreshToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(userId, out int id))
                return Unauthorized();

            var newToken = await _authService.RefreshToken(id,refreshToken);

            if(newToken == null)
                return NotFound(new {message = "user or token not found"});

            return Ok(newToken);
        }

    }
}