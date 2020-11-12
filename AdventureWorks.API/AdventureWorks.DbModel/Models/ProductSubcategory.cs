using System;
using System.Collections.Generic;

namespace AdventureWorks.DbModel.Models
{
    public class ProductSubcategoryDbModel
    {
        public ProductSubcategoryDbModel()
        {
            Product = new HashSet<ProductDbModel>();
        }

        public int ProductSubcategoryId { get; set; }
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategoryDbModel ProductCategory { get; set; }
        public virtual ICollection<ProductDbModel> Product { get; set; }
    }
}
