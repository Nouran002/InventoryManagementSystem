using Inventory.BL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services
{
    public class LowStockChecker : ILowStockChecker
    {
        private readonly IReportService _reportService;
        private readonly ILogger<LowStockChecker> _logger;

        public LowStockChecker(IReportService reportService, ILogger<LowStockChecker> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        public async Task CheckLowStockAsync()
        {
            var lowStockProducts = await _reportService.GetLowStockReportAsync();

            foreach (var product in lowStockProducts)
            {
                if (product.Quantity < product.LowStockThreshold)
                {
                    _logger.LogWarning($"Low stock detected: Product '{product.Name}' (Qty: {product.Quantity}) is below threshold ({product.LowStockThreshold})");
                }
            }
        }
    }

}
