using System.ComponentModel.DataAnnotations;

namespace PickupExpress.Core.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity in stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be greater than zero")]
        public int QuantityInStock { get; set; }
    }
}