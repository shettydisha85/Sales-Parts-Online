using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesPartsOnline.Models
{
    public class Product
    {
        public Guid productId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string name { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string description { get; set; }

        public string imagesURL { get; set; }

        public int stock { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }


    }
}
