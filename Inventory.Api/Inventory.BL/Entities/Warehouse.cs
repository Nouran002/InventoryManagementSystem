using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<WarehouseProduct> WarehouseProducts { get; set; }
        public ICollection<InventoryTransaction> inventoryTransactions { get; set; }
    }
}
