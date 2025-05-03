using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
        Task<IEnumerable<InventoryTransaction>> GetTransactionHistoryAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? productId,
        string transactionType);
    }
}
