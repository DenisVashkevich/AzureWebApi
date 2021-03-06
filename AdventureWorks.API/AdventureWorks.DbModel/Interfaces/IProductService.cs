﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.DbModel.Models;

namespace AdventureWorks.DbModel.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDbModel>> GetAllProductsAsync();
        Task<IEnumerable<ProductDbModel>> SearchForProductsAsync(params string[] keywords);
        Task<ProductDbModel> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(ProductDbModel product);
        Task ModifyProductAsync(ProductDbModel product);
        Task<bool> DeleteProductAsync(int id);
    }
}