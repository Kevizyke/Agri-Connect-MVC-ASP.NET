using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class RegisterFarmerDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
