using System;
using System.Collections.Generic;

namespace AdventureWorks.DbModel.Models
{
    public class ProductCategoryDbModel
    {
        public ProductCategoryDbModel()
        {
            ProductSubcategory = new HashSet<ProductSubcategoryDbModel>();
        }

        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductSubcategoryDbModel> ProductSubcategory { get; set; }
    }
}
