using System;
using System.Collections.Generic;

namespace AdventureWorks.DbModel.Models
{
    public partial class UnitMeasure
    {
        public UnitMeasure()
        {
            ProductSizeUnitMeasureCodeNavigation = new HashSet<Product>();
            ProductWeightUnitMeasureCodeNavigation = new HashSet<Product>();
        }

        public string UnitMeasureCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Product> ProductSizeUnitMeasureCodeNavigation { get; set; }
        public virtual ICollection<Product> ProductWeightUnitMeasureCodeNavigation { get; set; }
    }
}
