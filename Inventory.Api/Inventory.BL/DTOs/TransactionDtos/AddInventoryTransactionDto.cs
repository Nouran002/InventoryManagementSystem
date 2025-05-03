using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.TransactionDtos
{
    public class AddInventoryTransactionDto
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
