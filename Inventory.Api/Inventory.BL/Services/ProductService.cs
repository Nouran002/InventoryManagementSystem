using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Inventory.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> AddProductAsync(AddProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Quantity,
                Price = dto.Price,
                LowStockThreshold = dto.LowStockThreshold
            };

            await _repository.AddAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity
            };
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                LowStockThreshold = p.LowStockThreshold
            });
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var matchProduct = await _repository.GetByIdAsync(id);
            return new ProductDto
            {
                Id = matchProduct.Id,
                Name = matchProduct.Name,
                Quantity = matchProduct.Quantity
            };
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            existingProduct.Name = productDto.Name ?? existingProduct.Name;
            existingProduct.Description = productDto.Description ?? existingProduct.Description;
            existingProduct.Quantity = productDto.Quantity ?? existingProduct.Quantity;
            existingProduct.Price = productDto.Price ?? existingProduct.Price;
            existingProduct.LowStockThreshold = productDto.LowStockThreshold ?? existingProduct.LowStockThreshold;
            existingProduct.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(existingProduct);
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            await _repository.DeleteAsync(id);
        }

    }
}
