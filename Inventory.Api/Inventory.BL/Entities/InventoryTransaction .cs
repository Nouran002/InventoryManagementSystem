using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Entities
{
    public class InventoryTransaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int? WarehouseId { get; set; }


        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }
        [ForeignKey("UserId")]
        public User User  { get; set; }

    }
    public enum TransactionType
    {
        Add,
        Remove,
        Transfer
    }
}
