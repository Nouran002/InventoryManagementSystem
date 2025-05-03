using Inventory.BL.DTOs.StockDTO;
using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task AddAsync(InventoryTransaction transaction);
        Task UpdateAsync(InventoryTransaction transaction);
        Task<InventoryTransaction> GetByIdAsync(int id);
        Task<IEnumerable<InventoryTransaction>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<IEnumerable<InventoryTransaction>> GetTransactionsByProductIdAsync(int productId);
       

   }

}
