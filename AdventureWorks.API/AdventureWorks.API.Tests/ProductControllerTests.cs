using AutoMapper;
using AdventureWorks.API.Controllers;
using AdventureWorks.DbModel.Interfaces;
using Moq;
using Xunit;
using System.Threading.Tasks;
using AdventureWorks.DbModel.Models;
using AdventureWorks.API.Models;
using Microsoft.AspNetCore.Mvc;


namespace AdventureWorks.API.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public async void CreateProductShouldReturnBadrequestWhenRequestBodyIsNull()
        {
            //Arrange
            var productServieMock = new Mock<IProductService>();
            var mapperMock = new Mock<IMapper>();
            productServieMock.Setup(srvc => srvc.CreateProductAsync(new ProductDbModel())).Returns(async () => { return await Task.FromResult<bool>(true); });
            mapperMock.Setup(mpr => mpr.Map<ProductDbModel>(new ProductApiModel())).Returns(() => { return new ProductDbModel(); });
            var productController = new ProductController(productServieMock.Object, mapperMock.Object);

            //Act
            var result = await productController.CreateProduct(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateProductShouldReturnBadrequestWhenModelNotValid()
        {
            //Arrange
            var productServieMock = new Mock<IProductService>();
            var mapperMock = new Mock<IMapper>();
            productServieMock.Setup(srvc => srvc.CreateProductAsync(new ProductDbModel())).Returns(async () => { return await Task.FromResult<bool>(true); });
            mapperMock.Setup(mpr => mpr.Map<ProductDbModel>(new ProductApiModel())).Returns(() => { return new ProductDbModel(); });
            var productController = new ProductController(productServieMock.Object, mapperMock.Object);

            //Act
            var result = productController.CreateProduct(InvalidModel());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private ProductApiModel InvalidModel()
        {
            var validModel = new ProductApiModel()
            {
                ProductId = 1,
                Name = "Adjustable Race",
                ProductNumber = "AR-5381",
                MakeFlag = false,
                FinishedGoodsFlag = false,
                Color = "",
                SafetyStockLevel = 1000,
                ReorderPoint = 750,
                StandardCost = 0,
                ListPrice = 0,
                Size = "",
                SizeUnitMeasureCode = "",
                WeightUnitMeasureCode = "",
                Weight = 0,
                DaysToManufacture = 0,
                ProductLine = "",
                Class = "",
                Style = "",
                ProductSubcategoryId = 0,
                ProductModelId = 0,
                SellStartDate = new System.DateTime(year: 2008, month: 04, day: 30),
                SellEndDate = null,
                DiscontinuedDate = null,
                Rowguid = "694215b7-08f7-4c0d-acb1-d734ba44c0c8",
                ModifiedDate = System.DateTime.Parse("2014-02-08T10:01:36.827")
            };

            validModel.Name = string.Empty;

            return validModel;
        }
    }
}
