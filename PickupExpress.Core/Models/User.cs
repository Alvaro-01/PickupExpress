using System.ComponentModel.DataAnnotations;
namespace PickupExpress.Core.Models
{


    public enum UserRole
    {
        Customer,
        Employee
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid email address"),
        EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        public UserRole Role { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }


        //nav props
        public ICollection<Order>? Orders { get; set; }
    }
}