using System.ComponentModel.DataAnnotations;
using PickupExpress.Core.Models;

namespace PickupExpress.Core.DTOs
{
    public class OrderStatusUpdateDto
    {
        [Required(ErrorMessage = "New order status is required")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus NewStatus { get; set; }
    }
}