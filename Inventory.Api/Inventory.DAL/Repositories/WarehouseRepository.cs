using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.warehouseDTOS;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Inventory.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DAL.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly AppDbContext _context;

        public WarehouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<warehouseResponseDTO>> GetAllAsync()
        {
            return await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(wp => wp.Product)
                .Select(w => new warehouseResponseDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    Location = w.Location,
                    Products = w.WarehouseProducts.Select(wp => new ProductDto
                    {
                        Id = wp.Product.Id,
                        Name = wp.Product.Name,
                        Quantity = wp.Product.Quantity,
                        LowStockThreshold = wp.Product.LowStockThreshold,
                    }).ToList()
                })
                .ToListAsync();
        }


        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await _context.Warehouses
            .Include(w => w.WarehouseProducts)
            .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var warehouse = await GetByIdAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsInsideWarehouse(int warehouseId)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                    .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Id == warehouseId);

            if (warehouse == null)
                throw new Exception("Warehouse not found");

            return warehouse.WarehouseProducts.Select(wp => new ProductDto
            {
                Id = wp.Product.Id,
                Name = wp.Product.Name,
                Quantity = wp.Quantity,
                LowStockThreshold = wp.Product.LowStockThreshold
            });
        }

    }

}
