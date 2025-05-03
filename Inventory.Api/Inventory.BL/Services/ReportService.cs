using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.TransactionDtos;
using Inventory.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services
{
    public class ReportService:IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository repository)
        {
            _reportRepository = repository;
        }
        public async Task<IEnumerable<ProductDto>> GetLowStockReportAsync()
        {
            var products = await _reportRepository.GetLowStockProductsAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                LowStockThreshold = p.LowStockThreshold
            });
        }
        public async Task<IEnumerable<TransactionHistoryDto>> GetTransactionHistoryAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? productId,
        string transactionType)
        {
            var transactions = await _reportRepository.GetTransactionHistoryAsync(startDate, endDate, productId, transactionType);

            return transactions.Select(t => new TransactionHistoryDto
            {
                TransactionId = t.Id,
                ProductName = t.Product?.Name ?? string.Empty,
                TransactionType = t.TransactionType.ToString(),
                Quantity = t.Quantity,
                TransactionDate = t.TransactionDate,
                User = t.User?.Username ?? "Unknown"
            });
        }

    }
}
