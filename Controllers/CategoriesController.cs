using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Models.RequestModels;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();

            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if(category == null)
                return NotFound(new {message = "category not found"});

            return Ok(category);

        }
        [HttpGet("get-by-name")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByName([FromQuery] string name)
        {
            var category = await _categoryService.GetByName(name);

            if(category == null)
                return NotFound(new {message = "category not found"});

            return Ok(category);
        }
        [HttpPost("add-category")]
        public async Task<ActionResult> AddCategory(AddCategoryModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid name or image"});
        
            var isAdded = await _categoryService.AddCategory(request.Name,request.Image);

            if(isAdded == 0)
                return BadRequest(new {message = "invalid data"});

            return Ok(new {message = "added successfully"});
        }
        [HttpPut("update-category/{categoryId}")]
        public async Task<ActionResult> UpdateCategory(int categoryId,UpdateCategoryModel request)
        {
            var isUpdated = await _categoryService.UpdateCategory(categoryId,request.Name,request.Image);

            if(!isUpdated)
                return NotFound(new {message = "category not found"});

            return Ok(new {message = "updated successfully"});
        }
        [HttpDelete("delete-category/{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var isDeleted = await _categoryService.DeleteCategory(categoryId);

            if(!isDeleted)
                return NotFound(new {message = "category not found"});

            return Ok(new {message = "deleted successfully"});
        }
    }
}