using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesPartsOnline.DAL;
using SalesPartsOnline.Data;
using SalesPartsOnline.Models;
using ZstdSharp.Unsafe;

namespace SalesPartsOnline.Controllers
{
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion(1)]
    public class ProductController : ControllerBase
    {
        private readonly SPODbContext _dbContext;
        private readonly ILogger<ProductController> _logger;
        private readonly ICrudOperationsDL _crudoperationDL;

        public ProductController(SPODbContext context, ILogger<ProductController> logger, ICrudOperationsDL cRUDOperationsDL)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _crudoperationDL = cRUDOperationsDL ?? throw new ArgumentNullException(nameof(cRUDOperationsDL));
        }

        // GET: api/v1/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var productList = await _dbContext.Products.ToListAsync();
            if (productList == null)
            {
                throw new ArgumentNullException(nameof(Product));
            }

            return Ok(productList);
        }

        // GET api/v1/products/5
        [HttpGet("{id}", Name = "GetProductByID")]
        public IActionResult GetProductByID(Guid id)
        {
            try
            {
                var productToReturn = _dbContext.Products.FirstOrDefault(p => p.productId == id);
                if (productToReturn == null)
                {
                    _logger.LogError($"Product Id not found: {id}");
                    return NotFound();
                }

                return Ok(productToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred when Getting Product with Id: {id}", ex);
                return StatusCode(500, $"A problem happened while handling your request for Product Id: {id}");
            }
        }

        // POST api/v1/products
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductAddModel newProduct)
        {
            try
            {
                var productToAdd = new Product
                {
                    productId = Guid.NewGuid(),
                    name = newProduct.name,
                    price = newProduct.price,
                    description = newProduct.description,
                    imagesURL = newProduct.imageURL
                };

                _dbContext.Products.Add(productToAdd);
                await _dbContext.SaveChangesAsync();

                return CreatedAtRoute("GetProductByID", new { id = productToAdd.productId }, productToAdd);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred when Adding Product: {newProduct}", ex);
                return StatusCode(500, $"A problem happened while handling your request for Product: {newProduct}");
            }
        }

        // PUT api/v1/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductEditModel productUpdate)
        {
            try
            {
                var productToUpdate = _dbContext.Products.FirstOrDefault(p => p.productId == id);

                if (productToUpdate == null)
                {
                    return NotFound();
                }

                productToUpdate.description = productUpdate.description;
                productToUpdate.price = productUpdate.price;
                productToUpdate.stock = productUpdate.stock;

                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred when Updating Product with Id: {id}", ex);
                return StatusCode(500, $"A problem happened while handling your request for Product Id: {id}");
            }
        }

        // DELETE api/v1/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var productToDelete = _dbContext.Products.FirstOrDefault(x => x.productId == id);
                if (productToDelete == null)
                {
                    return NotFound();
                }

                _dbContext.Products.Remove(productToDelete);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred when Deleting Product with Id: {id}", ex);
                return StatusCode(500, $"A problem happened while handling your request for Product Id: {id}");
            }
        }

        // POST api/v1/products/mongodbtest
        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] InsertProductRequest productRequest)
        {
            InsertProductResponse response = new InsertProductResponse();

            try
            {
                response = await _crudoperationDL.InsertRecord(productRequest);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred when Inserting Product to MongoDB: {productRequest}", ex);
                response.IsSuccess = false;
                response.Message = "Product could not be created";
            }

            return Ok(response);
        }

        [HttpGet("GetDataByID")]

        public async Task<ActionResult<GetProductByIDResponse>> GetDataFromMongo([FromQuery] string id)
        {
            GetProductByIDResponse response = new GetProductByIDResponse();
            try
            {
                response = await _crudoperationDL.GetRecordByID(id);
                response.IsSuccess = true;

                return Ok(response);
                
            }
            catch 
            {
                response.IsSuccess = false;
                response.Message = "Error occurred";
                return StatusCode(500, response);
                
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFromMongo([FromQuery] string id)
        {
            InsertProductResponse response = new InsertProductResponse();
            try
            {
                response = await _crudoperationDL.DeleteRecord(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, response);
            }
        }

        [HttpPut("UpdateById")]
        public async Task<IActionResult> UpdateById(InsertProductRequest request)
        {
            InsertProductResponse response = new InsertProductResponse();
            try
            {
                response = await _crudoperationDL.UpdateRecord(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }
    }
}