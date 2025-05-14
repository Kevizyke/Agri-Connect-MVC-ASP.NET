namespace ProductService.Models
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }
        public string UserId { get; set; }  // Passed from the MVC app
    }
}
