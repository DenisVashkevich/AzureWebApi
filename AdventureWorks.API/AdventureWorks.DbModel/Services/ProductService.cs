using System.Collections.Generic;
using AdventureWorks.DbModel.Interfaces;
using AdventureWorks.DbModel.Models;
using AdventureWorks.DbModel.Context;
using System.Linq;
using AdventureWorks.DbModel.Utils;
using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.DbModel.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _dbcontext;

        public ProductService(ProductContext context)
        {
            Log.Information("Production service constructor.");
            _dbcontext = context;
        }

        public async Task<IEnumerable<ProductDbModel>> SearchForProductsAsync(params string[] keywords)
        {
            var predicate = PredicateBuilder.False<ProductDbModel>();

            foreach (string keyword in keywords)
            {
                predicate = predicate.Or(p => p.Name.Contains(keyword));
            }

            return await _dbcontext.Product.Where(predicate).ToListAsync();
        }

        public async Task<ProductDbModel> GetProductByIdAsync(int id)
        {
            var item = await _dbcontext.Product.FirstOrDefaultAsync(p => p.ProductId == id);

            if (item == null)
            {
                Log.Information($"product with id={id} not found in db");
            }

            return item;
        }

        public async Task CreateProductAsync(ProductDbModel product)
        {
            try
            {
                await _dbcontext.Product.AddAsync(product);
            }
            catch(System.Exception ex)
            {
                Log.Error(ex, $"Product id={product.ProductId} creation failed.");
            }
        }

        public async Task<IEnumerable<ProductDbModel>> GetAllProductsAsync()
        {
            Log.Information("Now you have everything!");

            return await _dbcontext.Product.ToListAsync();
        }


        public async Task ModifyProductAsync(ProductDbModel product)
        {
            var item = await _dbcontext.Product.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            if (item != null)
            {
                _dbcontext.Product.Update(product);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                Log.Warning("No product found in database.");
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var item = await _dbcontext.Product.FindAsync(id);

            if(item == null)
            {
                Log.Error($"Product with id={id} was not found in database, delete failed.");
                return false;
            }

            try
            {
                _dbcontext.Product.Remove(item);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }
        }
    }
}