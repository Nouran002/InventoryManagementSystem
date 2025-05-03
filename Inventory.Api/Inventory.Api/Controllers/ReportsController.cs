using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Inventory.BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [EnableRateLimiting("ReportPolicy")]
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockReport()
        {
            var report = await _reportService.GetLowStockReportAsync();
            return Ok(report);
        }


        [HttpGet("low-stock/pdf")]
        public async Task<IActionResult> GetLowStockReportPdf([FromServices] ReportPdfGeneratorService pdfGenerator)
        {
            var report = await _reportService.GetLowStockReportAsync();
            var pdfBytes = pdfGenerator.GenerateLowStockReport(report);

            return File(pdfBytes, "application/pdf", "LowStockReport.pdf");
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactionHistory(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? productId,
        [FromQuery] string transactionType)
        {
            var result = await _reportService.GetTransactionHistoryAsync(startDate, endDate, productId, transactionType);
            return Ok(result);
        }
    }
}
