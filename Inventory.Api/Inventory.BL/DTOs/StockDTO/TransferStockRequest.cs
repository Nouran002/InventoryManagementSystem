using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.StockDTO
{
    public class TransferStockRequest: StockRequest
    {
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
    }

}
