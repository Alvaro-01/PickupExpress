using System.ComponentModel.DataAnnotations;
namespace PickupExpress.Core.Models
{
    public class OrderItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        //nav props
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}