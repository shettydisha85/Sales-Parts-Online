using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using SalesPartsOnline.Models;

namespace SalesPartsOnline.Data
{
   
    public class CRUDOperationsDL : ICrudOperationsDL
    {
        private readonly IConfiguration configuration;
        private readonly MongoClient mongoClient;
        private readonly IMongoCollection<InsertProductRequest> _mongoCollection;
        public CRUDOperationsDL(IConfiguration configuration)
        {
            this.configuration = configuration;
            mongoClient = new MongoClient(configuration[key : "ProductDatabaseSettings:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(configuration[key: "ProductDatabaseSettings:Databasename"]);
            _mongoCollection = mongoDatabase.GetCollection<InsertProductRequest>(configuration[key: "ProductDatabaseSettings:ProductCollection"]);
        
        }

        public async Task<InsertProductResponse> InsertRecord(InsertProductRequest request)
        {
            InsertProductResponse response = new InsertProductResponse();
            try {

                request.createdDate = DateTime.Now.ToString();
                request.updatedDate = string.Empty;

                await _mongoCollection.InsertOneAsync(request);

                response.IsSuccess = true;

            }
            catch(Exception ex) {
                response.IsSuccess = false;
                response.Message = ex.Message + "Exception Occurred";
            }
            return response;
        }

        public async Task<GetProductByIDResponse> GetRecordByID(string id)
        {
            GetProductByIDResponse response = new GetProductByIDResponse();
            try
            {

                response.Data = await _mongoCollection.Find(x => (x.Id == id)).FirstOrDefaultAsync();

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message + "Exception Occurred";
            }
            return response;
        }
        public async Task<InsertProductResponse> DeleteRecord(string id)
        {
            InsertProductResponse response = new InsertProductResponse();
            try
            {

                await _mongoCollection.DeleteOneAsync(x => (x.Id == id));

                response.IsSuccess = true;
                response.Message = "Record Deleted";

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message + "Exception Occurred";
            }
            return response;
        }
        public async Task<InsertProductResponse> UpdateRecord(InsertProductRequest request)
        {
            InsertProductResponse response = new InsertProductResponse();
            try
            {
                request.updatedDate = DateTime.Now.ToString();

                await _mongoCollection.InsertOneAsync(request);

                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message + "Exception Occurred";
            }
            return response;
        }

    }
}
