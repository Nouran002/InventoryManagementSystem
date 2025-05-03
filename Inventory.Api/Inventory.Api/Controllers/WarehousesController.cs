using Inventory.BL.DTOs.warehouseDTOS;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var warehouses = await _warehouseService.GetAllAsync();
            return Ok(warehouses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null) return NotFound();
            return Ok(warehouse);
        }
        [HttpPost("Add-warehouse")]
        public async Task<IActionResult> Add([FromBody] WarehouseDto warehouseDto)
        {
            var warehouse = new Warehouse
            {
                Name = warehouseDto.Name,
                Location = warehouseDto.Location
            };

            await _warehouseService.AddAsync(warehouse);
            return  Ok(new { id = warehouse.Id });
        }


        [HttpPut("update-warehouse/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WarehouseDto warehouseDto)
        {
            var existingWarehouse = await _warehouseService.GetByIdAsync(id);
            if (existingWarehouse == null)
                return NotFound();

            existingWarehouse.Name = warehouseDto.Name;
            existingWarehouse.Location = warehouseDto.Location;

            var w = new Warehouse
            {
                Name = existingWarehouse.Name,
                Location = existingWarehouse.Location
            };

            await _warehouseService.UpdateAsync(w);
            return Ok(existingWarehouse);
        }

        [HttpDelete("delete-warehouse/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _warehouseService.DeleteAsync(id);
            return Ok("Warehouse Deleted Successfully");
        }
        [HttpGet("{warehouseId}/products")]
        public async Task<IActionResult> GetAllProductsInsideWarehouse(int warehouseId)
        {
            var products = await _warehouseService.GetAllProductsInsideWarehouse(warehouseId);
            return Ok(products);
        }
    }

}
