using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.DTOs;
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
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetProducts();

            if(products == null || !products.Any())
                return NotFound(new {message = "there is no products yet"});
            
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductWithReviews(id);

            if(product == null)
                return NotFound(new {message = "product not found"});

            return Ok(product);
        }
        [HttpGet("filter/{categoryId}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsFilteredByPrice(int categoryId, [FromQuery]int minPrice, [FromQuery]int? maxPrice = null)
        {
            var products = await _productService.GetProductsFiltredByPrice(categoryId,minPrice,maxPrice);

            if(products == null)
                return NotFound(new {message = "no products found"});

            return Ok(products);
        }
        [HttpGet("search/{name}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsByName(string name)
        {
            var products = await _productService.GetProductsByName(name);

            if(products == null)
                return NotFound(new {message = "there is no products with that name"});

            return Ok(products);
        }
        [HttpPost("{productId}")]
        public async Task<ActionResult<int>> AddProduct(int productId, [FromBody] int count)
        {
            var ProductId = await _productService.AddProduct(productId,count);

            if(ProductId == -1)
                return NotFound(new {message = "inevntory product not found"});
            
            if(ProductId == -2)
                return BadRequest(new {message = "product already exsists"});

            return Ok(ProductId);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isProductDeleted =  await _productService.Delete(id);

            if(!isProductDeleted)
                return NotFound(new {message = "product not found"});

            return Ok(new {message = "product deleted succefully"});
        }
    }
}