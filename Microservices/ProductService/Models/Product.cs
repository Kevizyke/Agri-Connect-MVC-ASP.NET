using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public DateTime ProductionDate { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required]
        public string UserId { get; set; }  // Reference to the user who created it

        public string? ImagePath { get; set; }  // Reference to the image path in the MVC app

    }
}
