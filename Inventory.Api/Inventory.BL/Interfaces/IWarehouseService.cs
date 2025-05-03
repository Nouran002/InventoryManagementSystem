using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.warehouseDTOS;
using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<warehouseResponseDTO>> GetAllAsync();
        Task<WarehouseDto> GetByIdAsync(int id);
        Task AddAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsInsideWarehouse(int warehouseId);
    }

}
