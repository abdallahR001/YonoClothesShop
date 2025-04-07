using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Models.RequestModels;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("")]
        public async Task<actionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productService.GetProducts();

            if(products == null)
                return null;
            
            return Ok(products);
        }
        [HttpPost("add-product")]
        public async Task<ActionResult<int>> AddProduct(AddProductModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid data"});
            var ProductId = await _productService.AddProduct(request.Name,request.Description,request.Image,request.Price,request.Count,request.CategoryId);
            
            if(ProductId == -1)
                return BadRequest(new {message = "product already exsists"});

            if(ProductId == -2)
                return BadRequest(new {message = "invalid data"});
            return Ok(ProductId);
        }
        [HttpPost("update-product/{id}")]
        public async Task<ActionResult<int>> UpdateProduct(int id,UpdateProductModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid data"});

            var productId = await _productService.UpdateProduct(id,request.Name,request.Description,request.Image,request.Price,request.Count);

            if(productId == -1)
                return NotFound(new {message = "product not found"});

            return Ok(productId);
        }
    }
}