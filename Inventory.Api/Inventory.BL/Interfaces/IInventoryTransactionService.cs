using Inventory.BL.DTOs.StockDTO;
using Inventory.BL.DTOs.TransactionDtos;
using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Interfaces
{
    public interface IInventoryTransactionService
    {
        Task<InventoryTransaction> AddTransactionAsync(AddInventoryTransactionDto transactionDto);
        Task<TransactionDTO> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync();
        Task<IEnumerable<TransactionDTO>> GetTransactionsByProductIdAsync(int productId);
        Task UpdateTransactionAsync(int id, UpdateInventoryTransactionDto transactionDto);

        Task DeleteTransactionAsync(int id);

        Task AddStockAsync(int productId, int quantity,int warehouseId, int userId);
        Task RemoveStockAsync(int productId, int quantity,int warehouseId, int userId);
        Task TransferStockAsync(int productId, int quantity, int fromWarehouseId, int toWarehouseId, int userId);
    }






}

