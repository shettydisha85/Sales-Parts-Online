using SalesPartsOnline.Models;

namespace SalesPartsOnline.Data
{
    public interface ICrudOperationsDL
    {
        public Task<InsertProductResponse> InsertRecord(InsertProductRequest request);
        public Task<GetProductByIDResponse> GetRecordByID(string id);
        public Task<InsertProductResponse> DeleteRecord(string id);
        public Task<InsertProductResponse> UpdateRecord(InsertProductRequest request);

    }
}
