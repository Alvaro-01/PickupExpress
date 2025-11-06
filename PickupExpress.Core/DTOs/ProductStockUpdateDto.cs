using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PickupExpress.Core.DTOs
{
    public class ProductStockUpdateDto
    {
        [Required(ErrorMessage = "New Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "New Quantity must be greater than or equal to zero")]
        public int NewQuantity { get; set; }
    }
}