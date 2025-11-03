using System.ComponentModel.DataAnnotations;

namespace PickupExpress.Core.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int QuantityInStock { get; set; }


        //nav props
        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}