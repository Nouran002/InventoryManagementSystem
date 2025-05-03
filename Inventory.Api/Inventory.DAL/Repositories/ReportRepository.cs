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
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.Quantity < p.LowStockThreshold)
                .ToListAsync();
        }
        public async Task<IEnumerable<InventoryTransaction>> GetTransactionHistoryAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? productId,
            string transactionType)
        {
            var query = _context.inventoryTransactions.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= endDate.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(t => t.ProductId == productId.Value);
            }

            if (!string.IsNullOrEmpty(transactionType))
            {
                if (Enum.TryParse<TransactionType>(transactionType, true, out var parsedType))
                {
                    query = query.Where(t => t.TransactionType == parsedType);
                }
            }

            return await query
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}

