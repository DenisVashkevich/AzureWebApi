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
            Log.Information("Controlle constructorr !!!!!!!!!!!!!");

            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public List<ProductDbModel> GetProducts()
        { 
            Log.Information("ROUTE: api/Product !!!!!!!!!!!!!");

            return _productService.GetAllProducts();
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public ProductDbModel GetProduct(int id)
        {
            return _productService.GetProductById(id);
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
