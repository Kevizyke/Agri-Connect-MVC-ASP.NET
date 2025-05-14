using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class ProductWithImageDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }
        public string? Image { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }

}
