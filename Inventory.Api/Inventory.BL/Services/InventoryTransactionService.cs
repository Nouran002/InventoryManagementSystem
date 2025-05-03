using Inventory.BL.DTOs.ProductDtos;
using Inventory.BL.DTOs.StockDTO;
using Inventory.BL.DTOs.TransactionDtos;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services
{
    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly IInventoryTransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository ;

        public InventoryTransactionService(IInventoryTransactionRepository transactionRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
        }
        public async Task<InventoryTransaction> AddTransactionAsync(AddInventoryTransactionDto transactionDto)
        {
            var transaction=new InventoryTransaction { 
              Quantity = transactionDto.Quantity,
              TransactionType = transactionDto.TransactionType,
              TransactionDate=DateTime.Now,
              UserId = transactionDto.UserId,
              ProductId=transactionDto.ProductId,
            
            };
            await _transactionRepository.AddAsync(transaction);
            return transaction;

        }


        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions= await _transactionRepository.GetAllAsync();
            return transactions.Select(t => new TransactionDTO
            {
                Quantity = t.Quantity,
                TransactionType = t.TransactionType,
                ProductId = t.ProductId,
            });
        }

        public async Task<TransactionDTO> GetTransactionByIdAsync(int id)
        {
            var matchedTransaction = await _transactionRepository.GetByIdAsync(id);

            if (matchedTransaction == null)
            {
                throw new KeyNotFoundException($"Transaction with ID {id} was not found ");
            }

            return new TransactionDTO
            {
                Quantity = matchedTransaction.Quantity,
                TransactionType = matchedTransaction.TransactionType,
                //User = matchedTransaction.User,
                ProductId = matchedTransaction.ProductId
            };
        }


        public async Task<IEnumerable<TransactionDTO>> GetTransactionsByProductIdAsync(int productId)
        {
            //return await _transactionRepository.GetTransactionsByProductIdAsync(productId);
            var transactions = await _transactionRepository.GetTransactionsByProductIdAsync(productId);

            return transactions.Select(t => new TransactionDTO
            {
                Quantity = t.Quantity,
                TransactionType = t.TransactionType,
               // User = t.User,
                ProductId = t.ProductId
            });

        }

        public async Task UpdateTransactionAsync(int id, UpdateInventoryTransactionDto transactionDto)
        {
            var existingTransaction=await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction==null)
            {
                throw new KeyNotFoundException($"Transaction with id {id} not found.");

            }

            existingTransaction.Quantity=transactionDto.Quantity?? existingTransaction.Quantity;
            existingTransaction.TransactionType = transactionDto.TransactionType ?? existingTransaction.TransactionType;
           // existingTransaction.User = transactionDto.User ?? existingTransaction.User;
            existingTransaction.TransactionDate = transactionDto.TransactionDate ?? existingTransaction.TransactionDate;
            existingTransaction.ProductId = transactionDto.ProductId ?? existingTransaction.ProductId;

            await _transactionRepository.UpdateAsync(existingTransaction);
        }
        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with id {id} not found.");
            }

            await _transactionRepository.DeleteAsync(id);
        }


        public async Task AddStockAsync(int productId, int quantity, int warehouseId, int userId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            if (warehouse == null)
                throw new Exception("Warehouse not found");

            var warehouseProduct = warehouse.WarehouseProducts
                .FirstOrDefault(wp => wp.ProductId == productId);

            if (warehouseProduct == null)
            {
                warehouseProduct = new WarehouseProduct
                {
                    ProductId = productId,
                    WarehouseId = warehouseId,
                    Quantity = quantity
                };
                warehouse.WarehouseProducts.Add(warehouseProduct);
            }
            else
            {
                warehouseProduct.Quantity += quantity;
            }
            product.Quantity += quantity;

            await _productRepository.UpdateAsync(product);
            await _warehouseRepository.UpdateAsync(warehouse);

            await _transactionRepository.AddAsync(new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                TransactionType = TransactionType.Add,
                TransactionDate = DateTime.Now,
                UserId = userId,
                WarehouseId = warehouseId
            });
        }




        public async Task RemoveStockAsync(int productId, int quantity, int warehouseId, int userId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            if (warehouse == null)
                throw new Exception("Warehouse not found");

            var warehouseProduct = warehouse.WarehouseProducts
                .FirstOrDefault(wp => wp.ProductId == productId);

            if (warehouseProduct == null)
                throw new Exception("Product not found in this warehouse");

            if (warehouseProduct.Quantity < quantity)
                throw new Exception("Not enough stock in the warehouse");

            warehouseProduct.Quantity -= quantity;
            if (product.Quantity < quantity)
                throw new Exception("Not enough global stock");

            product.Quantity -= quantity;

            await _productRepository.UpdateAsync(product);

            await _warehouseRepository.UpdateAsync(warehouse);

            await _transactionRepository.AddAsync(new InventoryTransaction
            {
                ProductId = productId,
                Quantity = -quantity,
                TransactionType = TransactionType.Remove,
                TransactionDate = DateTime.UtcNow,
                UserId = userId,
                WarehouseId = warehouseId
            });
        }


        public async Task TransferStockAsync(int productId, int quantity, int fromWarehouseId, int toWarehouseId, int userId)
        {
            var fromWarehouse = await _warehouseRepository.GetByIdAsync(fromWarehouseId);
            if (fromWarehouse == null) 
                throw new Exception("Source warehouse not found");

            var toWarehouse = await _warehouseRepository.GetByIdAsync(toWarehouseId);
            if (toWarehouse == null) 
                throw new Exception("Destination warehouse not found");

            var fromWarehouseProduct = fromWarehouse.WarehouseProducts
                .FirstOrDefault(wp => wp.ProductId == productId);
            if (fromWarehouseProduct == null || fromWarehouseProduct.Quantity < quantity)
                throw new Exception("Not enough stock in source warehouse.");

            fromWarehouseProduct.Quantity -= quantity;

            var toWarehouseProduct = toWarehouse.WarehouseProducts
                .FirstOrDefault(wp => wp.ProductId == productId);
            if (toWarehouseProduct == null)
            {
                toWarehouseProduct = new WarehouseProduct
                {
                    ProductId = productId,
                    WarehouseId = toWarehouseId,
                    Quantity = quantity
                };
                toWarehouse.WarehouseProducts.Add(toWarehouseProduct);
            }
            else
            {
                toWarehouseProduct.Quantity += quantity;
            }

            await _warehouseRepository.UpdateAsync(fromWarehouse);
            await _warehouseRepository.UpdateAsync(toWarehouse);

            await _transactionRepository.AddAsync(new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                TransactionType = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow,
                UserId = userId,
                WarehouseId = fromWarehouseId 
            });

            await _transactionRepository.AddAsync(new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                TransactionType = TransactionType.Transfer,
                TransactionDate = DateTime.UtcNow,
                UserId = userId,
                WarehouseId = toWarehouseId 
            });
        }





    }
}
