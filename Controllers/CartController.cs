using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using YonoClothesShop.Models.RequestModels;
using System.Security.Claims;
using YonoClothesShop.DTOs;
namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("{productId}"),Authorize]
        public async Task<ActionResult> AddToCart(int productId, AddProductToCartModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "bad data"});
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isAddedToCart = await _cartService.AddProductToCart(id,productId,request.Quantity);

            if(!isAddedToCart)
                return NotFound(new {message = "failed to add to cart because user or cart was not found"});

            return Ok(new {message = "added successfully"});
        }
        [HttpGet(""),Authorize]
        public async Task<ActionResult<CartDTO>> ViewCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var cart = await _cartService.ViewCart(id);

            if(cart == null)
                return NotFound(new {message = "cart not found"});

            return Ok(cart);
        }
        [HttpGet("checkout"),Authorize]
        public async Task<ActionResult> Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var result = await _cartService.Checkout(id);

            if(!result)
                return NotFound(new {message = "user or cart or cart items not found"});
            return Ok("placed order successfully");
        }
        [HttpDelete("{productId}"),Authorize]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isRemoved = await _cartService.RemoveProductFromCart(id,productId);

            if(isRemoved == 0)
                return NotFound(new {message = "cart not found"});

            if(isRemoved == -1)
                return NotFound(new {message = "cart is empty"});

            if(isRemoved == -2)
                return NotFound(new {message = "product not found"});

            return Ok(new {message = "removed successfully"});
        }
        [HttpDelete("clear"),Authorize]
        public async Task<ActionResult> ClearCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(!int.TryParse(userId,out int id))
                return Forbid();

            var isCleared = await _cartService.ClearCart(id);

            if(!isCleared)
                return NotFound(new {message = "cart not found"});

            return Ok(new {message = "deleted successfully"});
        }
    }
}