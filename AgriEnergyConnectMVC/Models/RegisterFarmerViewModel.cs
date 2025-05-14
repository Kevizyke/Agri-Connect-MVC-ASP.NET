using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnectMVC.Models
{
    public class RegisterFarmerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
