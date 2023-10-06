using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class GetStockAtFixedDateShould
    {

        [TestMethod]
        public void ReturnEmptyListOfProductsIfNoProducts()
        {
            //Arrange
            var sut = new Stock();
            //Act
            var result = sut.GetStockAtFixedDate(DateTime.Today);
            //Assert
            Assert.IsTrue(result.Count == 0);
        }
        
        
        [TestMethod]
        public void ReturnProductsInStock()
        {
            //Arrange
            var sut = new Stock();
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), new EanId("EAN00001"), 2);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), new EanId("EAN00002"), 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), new EanId("EAN00003"), 4);
            //Act
            var result = sut.GetStockAtFixedDate(DateTime.Today);
            //Assert
            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result[0].EanId == new EanId("EAN00001") && result[0].Quantity == 2);
            Assert.IsTrue(result[1].EanId == new EanId("EAN00002") && result[1].Quantity == 3);
            Assert.IsTrue(result[2].EanId == new EanId("EAN00003") && result[2].Quantity == 4);
        }
        
        [TestMethod]
        public void ReturnProductsInStockIgnoringMovementAfterDate()
        {
            //Arrange
            var sut = new Stock();
            sut.AddInventoryMovement(DateTime.Today.AddDays(-7), new EanId("EAN00001"), 2);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), new EanId("EAN00002"), 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(3), new EanId("EAN00005"), 8);
            sut.AddProductMovement(DateTime.Today.AddDays(-3), "CMD", new EanId("EAN00001"), 2);
            sut.AddProductMovement(DateTime.Today, "CMD2", new EanId("EAN00001"), 7);
            
            //Act
            var result = sut.GetStockAtFixedDate(DateTime.Today.AddDays(-1));
            
            //Assert
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].EanId == new EanId("EAN00001") && result[0].Quantity == 4);
            Assert.IsTrue(result[1].EanId == new EanId("EAN00002") && result[1].Quantity == 3);
        }
        
    }
}