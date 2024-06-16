namespace SalesPartsOnline.Models
{
    public class GetProductByIDResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public InsertProductRequest Data { get; set; }
    }
}
