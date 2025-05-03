using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.warehouseDTOS;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<warehouseResponseDTO>> GetAllAsync()
        {
            return await _warehouseRepository.GetAllAsync();
        }

        public async Task<WarehouseDto> GetByIdAsync(int id)
        {
            Warehouse warehouse = await _warehouseRepository.GetByIdAsync(id);
            return new WarehouseDto
            {
                Name = warehouse.Name,
                Location = warehouse.Location
            };
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            await _warehouseRepository.AddAsync(warehouse);
        }

        public async Task UpdateAsync(Warehouse warehouse)
        {
            await _warehouseRepository.UpdateAsync(warehouse);
        }

        public async Task DeleteAsync(int id)
        {
            await _warehouseRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsInsideWarehouse(int warehouseId)
        {
            return await _warehouseRepository.GetAllProductsInsideWarehouse(warehouseId);
        }
    }

}
