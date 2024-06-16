using System.ComponentModel.DataAnnotations;
using static SalesPartsOnline.Models.Product;

namespace SalesPartsOnline.Models
{
    public class ProductEditModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string name { get; set; }

        [Required(ErrorMessage = "Category is required")]

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string description { get; set; }
        [Required]
        public int stock { get; set; }
        [Required]
        public string imageURL { get; set; }


    }
}
