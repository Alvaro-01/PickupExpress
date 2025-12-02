using System.ComponentModel.DataAnnotations;
using PickupExpress.Core.Models;

namespace PickupExpress.Core.DTOs
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Order status is required")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime OrderDate { get; set; }
    }
}