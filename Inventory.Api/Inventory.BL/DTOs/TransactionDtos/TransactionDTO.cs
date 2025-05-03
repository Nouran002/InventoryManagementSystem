using Inventory.BL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.TransactionDtos
{
    public class TransactionDTO
    {
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        
        public string User { get; set; }
        public int ProductId { get; set; }
    }
}
