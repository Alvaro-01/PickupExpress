using System.ComponentModel.DataAnnotations;

namespace PickupExpress.Core.DTOs
{
    public class OrderItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
