using System;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class StockTest
    {
        [TestMethod]
        public void StockHasAnEmptyListOfProductsOnCreation()
        {
            //Arrange
            //Act
            var sut = new Stock();
            //Assert
            Assert.IsTrue(!sut.Products.Any());
        }
        
        [TestMethod]
        public void StockHasAnEmptyListOfMovementsOnCreation()
        {
            //Arrange
            //Act
            var sut = new Stock();
            //Assert
            Assert.IsTrue(!sut.MovementHistory.Any());
        }
        
    }
}
