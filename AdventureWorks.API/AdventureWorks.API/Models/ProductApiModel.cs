using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.API.Models
{
    public class ProductApiModel
    {
        [Required(ErrorMessage = "Id required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ProductNumber required")]
        public string ProductNumber { get; set; }

        public bool? MakeFlag { get; set; }
        public bool? FinishedGoodsFlag { get; set; }
        public string Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public string SizeUnitMeasureCode { get; set; }
        public string WeightUnitMeasureCode { get; set; }
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string ProductLine { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public int? ProductSubcategoryId { get; set; }
        public int? ProductModelId { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public string Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
