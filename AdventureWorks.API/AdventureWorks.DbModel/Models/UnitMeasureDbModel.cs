using System;
using System.Collections.Generic;

namespace AdventureWorks.DbModel.Models
{
    public class UnitMeasureDbModel
    {
        public UnitMeasureDbModel()
        {
            ProductSizeUnitMeasureCodeNavigation = new HashSet<ProductDbModel>();
            ProductWeightUnitMeasureCodeNavigation = new HashSet<ProductDbModel>();
        }

        public string UnitMeasureCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductDbModel> ProductSizeUnitMeasureCodeNavigation { get; set; }
        public virtual ICollection<ProductDbModel> ProductWeightUnitMeasureCodeNavigation { get; set; }
    }
}
