using System.ComponentModel.DataAnnotations;

namespace PickupExpress.Core.DTOs
{
    public class UserAddDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }  // Plain password, to be hashed in service layer

        [Required]
        public Models.UserRole Role { get; set; }

    }
}