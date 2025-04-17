using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models.RequestModels;

namespace YonoClothesShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet("")]
        public async Task<ActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetSuppliers();

            return Ok(suppliers);
        }
        [HttpPost("add-supplier")]
        public async Task<ActionResult> AddSupplier(AddSupplierModel request)
        {
            if(!ModelState.IsValid)
                return BadRequest(new {message = "invalid data"});
            var result = await _supplierService.Add(request.Name,request.CompanyName,request.PhoneNumber);

            if(!result)
                return BadRequest(new {message = "invalid data"});

            return Ok(new {message = "added successfully"});
        }
    }
}