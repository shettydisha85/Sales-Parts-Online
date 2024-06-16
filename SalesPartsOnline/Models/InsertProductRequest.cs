using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SalesPartsOnline.Models
{
    public class InsertProductRequest
    {
        [BsonId]
       [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Guid productId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [BsonElement(elementName: "Name")]
        public string name { get; set; }


        public string createdDate { get; set; }
        public string updatedDate { get; set; }

    }
    public class InsertProductResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

    }
}

