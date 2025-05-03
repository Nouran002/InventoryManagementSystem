using Inventory.BL.DTOs.StockDTO;
using Inventory.BL.DTOs.TransactionDtos;
using Inventory.BL.Interfaces;
using Inventory.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactionController : Controller
    {
       private readonly IInventoryTransactionService _transactionService;

        public InventoryTransactionController(IInventoryTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("Add-Transaction")]
        public async Task<IActionResult> AddTransaction(AddInventoryTransactionDto transactionDto)
        {
            var result = await _transactionService.AddTransactionAsync(transactionDto);
            return Ok(result);
        }

        [HttpGet(" GetAll-Transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("get-transaction/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound($"Transaction with Id {id} not found.");

            return Ok(transaction);
        }

       
        [HttpGet("get-transactions-by-product/{productId}")]
        public async Task<IActionResult> GetTransactionsByProductId(int productId)
        {
            var transactions = await _transactionService.GetTransactionsByProductIdAsync(productId);
            return Ok(transactions);
        }

       
        [HttpPut("update-transaction/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, UpdateInventoryTransactionDto transactionDto)
        {
            try
            {
                await _transactionService.UpdateTransactionAsync(id, transactionDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete-transaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("add-stock")]
        public async Task<IActionResult> AddStock([FromBody] StockRequest request)
        {
            await _transactionService.AddStockAsync(request.ProductId, request.Quantity,request.warehouseId, request.UserId);
            return Ok(new { Message = "Stock added successfully." });
        }

        [HttpPost("remove-stock")]
        public async Task<IActionResult> RemoveStock([FromBody] StockRequest request)
        {
            await _transactionService.RemoveStockAsync(request.ProductId, request.Quantity,request.warehouseId, request.UserId);
            return Ok(new { Message = "Stock removed successfully." });
        }

        [HttpPost("transfer-stock")]
        public async Task<IActionResult> TransferStock([FromBody] TransferStockRequest request)
        {
            await _transactionService.TransferStockAsync(request.ProductId, request.Quantity, request.FromWarehouseId, request.ToWarehouseId, request.UserId);
            return Ok(new { Message = "Stock transferred successfully." });
        }


    }
}
