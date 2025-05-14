using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnectMVC.Models
{
    public class AddProductViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }


}
