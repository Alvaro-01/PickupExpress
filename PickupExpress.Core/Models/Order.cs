using System.ComponentModel.DataAnnotations;

namespace PickupExpress.Core.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Order status is required")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }

        //nav props
        public User? Customer { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}