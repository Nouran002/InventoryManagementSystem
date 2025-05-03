using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public int? LowStockThreshold { get; set; }
    }
}
