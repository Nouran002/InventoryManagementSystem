using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.TransactionDtos
{
    public class UpdateInventoryTransactionDto
    {
        public int? Quantity { get; set; }
        public TransactionType? TransactionType { get; set; }
        public string User { get; set; }
    
        public DateTime? TransactionDate { get; set; }
        public int? ProductId { get; set; }

       
    }
}
