using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.DbModel.Interfaces;
using AdventureWorks.DbModel.Models;
using Serilog;

namespace AdventureWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public List<ProductDbModel> GetProducts()
        { 
            Log.Information("Whole datatable transfered.");

            return _productService.GetAllProducts();
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public ProductDbModel GetProduct(int id)
        {
            var item = _productService.GetProductById(id);

            if(item == null)
            {
                Log.Warning($"Item not found id= {id}.");
            }

            return item;
        }

        // POST: api/Product
        [HttpPost]
        public void CreateProduct([FromBody] string value)
        {

            //_productService.ModifyProduct()
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void ModifyProduct(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteProduct(int id)
        {
        }
    }
}
