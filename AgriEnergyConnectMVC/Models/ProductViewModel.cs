using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnectMVC.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Comes from the JWT
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImagePath { get; set; }  
    }


}
