using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnectMVC.Models
{
    public class ProductFilterViewModel
    {
        public string? Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public string? FarmerId { get; set; }

        public List<FarmerViewModel>? Farmers { get; set; } // Contains a list of all the farmers

        public List<ProductViewModel> Results { get; set; } = new(); // This will contain products with image paths

    }


}
