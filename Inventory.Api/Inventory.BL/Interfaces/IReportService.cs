using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.TransactionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ProductDto>> GetLowStockReportAsync();
        Task<IEnumerable<TransactionHistoryDto>> GetTransactionHistoryAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? productId,
        string transactionType);
    }
}
