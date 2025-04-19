using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetById(id);

            if(supplier == null)
                return NotFound(new {message = "supplier not found"});

            return Ok(supplier);
        }
        [HttpPost("get-by-shipments-count")]
        public async Task<ActionResult<List<SupplierDTO>>> GetSuppliersByShipmintsCount([FromQuery] int min, [FromQuery] int? max=null)
        {
            var suppliers = await _supplierService.GetSuppliersByDeleveriesCount(min,max);

            if(!suppliers.Any())
                return NotFound(new {message = "no suppliers found"});

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
        [HttpPut("update-supplier/{id}")]
        public async Task<ActionResult> UpdateSupplier(int id, UpdateSupplierModel request)
        {
            var isUpdated = await _supplierService
            .Update(id,request.Name,request.CompanyName,request.PhoneNumber);

            if(!isUpdated)
                return NotFound(new {message = "supplier not found"});

            return Ok(new {message = "updated supplier successfully"});
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var isDeleted = await _supplierService.Delete(id);

            if(!isDeleted)
                return NotFound(new {message = "supplier not found"});

            return Ok(new {message = "deleted successfully"});
        }
    }
}