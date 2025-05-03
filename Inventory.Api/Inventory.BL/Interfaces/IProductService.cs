using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddProductAsync(AddProductDto dto);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductById(int  id);
        Task UpdateProductAsync(int id, UpdateProductDto productDto);
        Task DeleteProductAsync(int id);

    }
}
