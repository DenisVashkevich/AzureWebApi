using System.Collections.Generic;
using System.Linq;
using AdventureWorks.DbModel.Models;

namespace AdventureWorks.DbModel.Interfaces
{
    public interface IProductService
    {
        List<ProductDbModel> GetAllProducts();
        IQueryable<ProductDbModel> SearchForProducts(params string[] keywords);
        ProductDbModel GetProductById(int id);
        void CreateProduct(ProductDbModel product);
        void ModifyProduct(ProductDbModel product);
        void DeleteProduct(int id);
    }
}