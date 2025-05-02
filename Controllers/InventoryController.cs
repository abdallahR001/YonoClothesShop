using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models.RequestModels;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        [HttpGet("")]
        public async Task<ActionResult<List<InventoryDTO>>> GetInventoryProducts()
        {
            return Ok(await _inventoryService.GetProducts());
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult<InventoryDTO>> GetProduct(int productId)
        {
            var product = await _inventoryService.GetProduct(productId);

            if(product == null)
                return NotFound(new {message = "product not found"});

            return Ok(product);
        }
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<InventoryDTO>> GetProductsByCategory(int categoryId)
        {
            var products = await _inventoryService.GetProductsByCategory(categoryId);

            if(products == null)
                return NotFound(new {message = "category not found"});

            return Ok(products);
        }
        [HttpGet("search/{name}")]
        public async Task<ActionResult<InventoryDTO>> GetProductsByName(string name)
        {
            var products = await _inventoryService.GetProductsByName(name);

            if(products == null)
                return NotFound(new {message = "category not found"});

            return Ok(products);
        }
        [HttpGet("filter/{categoryId}")]
        public async Task<ActionResult<InventoryDTO>> GetProductsByCategory(int categoryId,[FromQuery] int minPrice, [FromQuery] int? maxPrice = null)
        {
            var products = await _inventoryService.GetProductsFiltredByPrice(categoryId,minPrice,maxPrice);

            if(products == null)
                return NotFound(new {message = "category not found"});

            return Ok(products);
        }
        [HttpPost("{categoryId}")]
        public async Task<ActionResult> AddProduct(int categoryId,AddInventoryProductModel request)
        {
            var isAdded = await _inventoryService.AddProduct(request.Name,request.Description,request.Image,request.Price,request.SupplierPrice,request.Count,categoryId,request.SupplierName,request.SupplierCompany);

            if(isAdded == -2)
                return BadRequest(new {message = "invalid data"});
            
            if(isAdded == 0)
                return NotFound(new {message = "category not found"});

            if(isAdded == -3)
                return NotFound(new {message = "supplier not found"});

            if(isAdded == -1)
                return BadRequest(new {message = "product already exist"});

            return Ok(new {message = $"product {request.Name} added successfully"});
        }
        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(int productId,UpdateInventoryProductModel request)
        {
            var isUpdated = await _inventoryService.UpdateProduct(productId,request.Name,request.Description,request.Image,request.Price,request.Count);

            if(isUpdated == -1)
                return BadRequest(new {message = "product not found"});

            return Ok(new {message = $"product updated successfully"});
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _inventoryService.Delete(id);

            if(!isDeleted)
                return NotFound(new {message = "product not found"});

            return Ok(new {message = "deleted successfully"});
        }
    }
}