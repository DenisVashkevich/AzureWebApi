﻿using System.Collections.Generic;
using AdventureWorks.DbModel.Interfaces;
using AdventureWorks.DbModel.Models;
using AdventureWorks.DbModel.Context;
using System.Linq;
using AdventureWorks.DbModel.Utils;
using Serilog;

namespace AdventureWorks.DbModel.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _dbcontext;

        public ProductService(ProductContext context)
        {
            _dbcontext = context;
        }

        public IQueryable<ProductDbModel> SearchForProducts(params string[] keywords)
        {
            var predicate = PredicateBuilder.False<ProductDbModel>();

            foreach (string keyword in keywords)
                predicate = predicate.Or(p => p.Name.Contains(keyword));

            return _dbcontext.Product.Where(predicate);
        }

        public ProductDbModel GetProductById(int id)
        {
            return _dbcontext.Product.FirstOrDefault(p => p.ProductId == id);
        }

        public void CreateProduct(ProductDbModel product)
        {
            try
            {
                _dbcontext.Product.Add(product);
            }
            catch(System.Exception ex)
            {
                Log.Error(ex, $"Product id={product.ProductId} creation failed.");
            }
            
        }

        public List<ProductDbModel> GetAllProducts()
        {
            return _dbcontext.Product.ToList();
        }


        void IProductService.ModifyProduct(ProductDbModel product)
        {
            var item = _dbcontext.Product.FirstOrDefault(p => p.ProductId == product.ProductId);

            if (item != null)
            {
                _dbcontext.Product.Update(product);
                _dbcontext.SaveChanges();
            }
        }

        void IProductService.DeleteProduct(int id)
        {
            var item = _dbcontext.Product.Find(id);
            _dbcontext.Product.Remove(item);
            _dbcontext.SaveChanges();
        }
    }
}