using Inventory.BL.DTOs.StockDTO;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Inventory.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Inventory.DAL.Repositories
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly AppDbContext _context;

        public InventoryTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryTransaction transaction)
        {
            await _context.inventoryTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<InventoryTransaction> GetByIdAsync(int id)
        {
            return await _context.inventoryTransactions.FindAsync(id);
        }

        public async Task<IEnumerable<InventoryTransaction>> GetAllAsync()
        {
            return await _context.inventoryTransactions.ToListAsync();
        }

        public async Task UpdateAsync(InventoryTransaction transaction)
        {
            _context.inventoryTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _context.inventoryTransactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsByProductIdAsync(int productId)
        {
            return await _context.inventoryTransactions
                                 .Where(t => t.ProductId == productId)
                                 .ToListAsync();
        }

  

    }
}
