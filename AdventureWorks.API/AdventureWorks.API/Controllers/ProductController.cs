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
using Microsoft.AspNetCore.Http;

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

        /// <summary>
        /// Get all products from DB.
        /// </summary>
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

        /// <summary>
        /// Deletes a specific product from DB.
        /// </summary>
        /// <param name="id"></param>     
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

        /// <summary>
        /// Search for product by names in comma separated string.
        /// </summary>
        /// <param name="names"></param>
        [HttpPost]
        [Route("/api/search")]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDbModel>> GetProducts([FromBody] string names)
        {
            var searchWords = names.Split(',');
            var items = await _productService.SearchForProductsAsync(searchWords);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        /// <summary>
        /// Create new product.
        /// </summary>
        /// <param name="value"></param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<ActionResult> CreateProduct(ProductApiModel item)
        {
            //var product = JsonConvert.DeserializeObject<ProductApiModel>(value);

            var result = await _productService.CreateProductAsync(_automaper.Map<ProductDbModel>(item));

            if(result)
            {
                return CreatedAtRoute("Product", new { id = item.ProductId }, item);
            }

            return BadRequest();
        }

        /// <summary>
        /// Modify existing product.
        /// </summary>
        /// <param name="value"></param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<ActionResult> ModifyProduct(ProductApiModel item)
        {
            await _productService.ModifyProductAsync(_automaper.Map<ProductDbModel>(item));

            return Ok();
        }

        /// <summary>
        /// Delete product in DB.
        /// </summary>
        /// <param name="id"></param>
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
