using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression("^(Employee|Farmer)$", ErrorMessage = "Role must be either 'Employee' or 'Farmer'.")]
        public string Role { get; set; } = "Farmer"; // Default role
    }
}
