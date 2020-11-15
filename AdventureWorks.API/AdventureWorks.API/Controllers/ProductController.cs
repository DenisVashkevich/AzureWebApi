using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.DbModel.Interfaces;
using AdventureWorks.DbModel.Models;
using AutoMapper;
using AdventureWorks.API.Models;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System;
using Newtonsoft.Json;

namespace AdventureWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _automaper;

        public ProductController(IProductService productService, IMapper automapper)
        {
            _productService = productService;
            _automaper = automapper;
        }

        // GET: api/Product
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ProductApiModel>>> GetProducts()
        {
            var items = (await _productService.GetAllProductsAsync()).Select(i => _automaper.Map<ProductApiModel>(i));

            if (items == null)
            {
                return NoContent();
            }

            return Ok(items);
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDbModel>> GetProduct(int id)
        {
            var item = await _productService.GetProductByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // GET: api/Products
        [HttpGet("search")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<ProductDbModel>> GetProducts([FromBody] string value)
        {
            var searchWords = JsonConvert.DeserializeObject<string[]>(value);
            var items = await _productService.SearchForProductsAsync(searchWords);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        // POST: api/Product
        [HttpPost]
        //[Consumes("application/json")]
        public async Task<ActionResult> CreateProduct([FromBody] string value)
        {
            Log.Information("Create JSON : "+ value);
            var product = JsonConvert.DeserializeObject<ProductApiModel>(value);
            var dtoModel = _automaper.Map<ProductDbModel>(product);

            try
            {
                await _productService.CreateProductAsync(dtoModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Product id={dtoModel.ProductId} creation failed");
                return StatusCode(500);
            }

            return Ok();
        }

        // PUT: api/Product
        [HttpPut]
        public async Task<ActionResult> ModifyProduct([FromBody] string value)
        {
            var product = JsonConvert.DeserializeObject<ProductApiModel>(value);
            var dtoModel = _automaper.Map<ProductDbModel>(product);

            try
            {
                await _productService.ModifyProductAsync(dtoModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Product id={dtoModel.ProductId} modification failed");
                return StatusCode(500);
            }

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            bool result;
            try
            {
                result = await _productService.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Product id={id} deletion failed");
                return StatusCode(500);
            }

            if(result)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
