using System;
using System.Collections.Generic;

namespace AdventureWorks.DbModel.Models
{
    public class ProductModelDbModel
    {
        public ProductModelDbModel()
        {
            Product = new HashSet<ProductDbModel>();
        }

        public int ProductModelId { get; set; }
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public string Instructions { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductDbModel> Product { get; set; }
    }
}
