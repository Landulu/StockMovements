using System;
using System.Collections.Generic;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class GetStockVariationBetweenDatesShould
    {

        [TestMethod]
        public void ReturnEmptyListIfStockHasNoMovements()
        {
            //Arrange
            var sut = new Stock();
            //Act
            var variations = sut.GetStockVariationBetweenDates(DateTime.MinValue, DateTime.MaxValue);
            //Assert
            Assert.IsNotNull(variations);
            Assert.AreEqual(0, variations.Count);
        }

        [TestMethod]
        public void ReturnEveryVariationsIfAllMovementsAreContainedBetweenStartAndEnd()
        {
            //Arrange
            
            var sut = new Stock();
            var firstId = new EanId("EAN00001");
            var secondId = new EanId("EAN00002");
            var thirdId = new EanId("EAN00003");
            sut.AddInventoryMovement(DateTime.Today.AddDays(-7), firstId, 2);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), secondId, 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), thirdId, 8);
            sut.AddProductMovement(DateTime.Today.AddDays(-3), "CMD", firstId, 2);
            sut.AddProductMovement(DateTime.Today, "CMD2", firstId, 7);
            sut.AddMultipleProductMovement(DateTime.Today, "CMD3", new List<ProductMovement>()
            {
                new ProductMovement(secondId, -3 ),
                new ProductMovement(thirdId, -1 ),
            });
            
            //Act
            var variations = sut.GetStockVariationBetweenDates(DateTime.MinValue, DateTime.MaxValue);

            //Assert
            Assert.AreEqual(3,variations.Count);
            Assert.IsTrue(variations.Any( v => v.EanId == firstId && v.Quantity == 11));
            Assert.IsTrue(variations.Any( v => v.EanId == secondId && v.Quantity == 0));
            Assert.IsTrue(variations.Any( v => v.EanId == thirdId && v.Quantity == 7));
        }
        
        [TestMethod]
        public void ReturnVariationsOfZeroIfAllMovementsAreOutsideOfStartAndEnd()
        {
            //Arrange
            
            var sut = new Stock();
            var firstId = new EanId("EAN00001");
            var secondId = new EanId("EAN00002");
            var thirdId = new EanId("EAN00003");
            sut.AddInventoryMovement(DateTime.Today.AddDays(-7), firstId, 2);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), secondId, 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), thirdId, 8);
            sut.AddProductMovement(DateTime.Today.AddDays(-3), "CMD", firstId, 2);
            sut.AddProductMovement(DateTime.Today, "CMD2", firstId, 7);
            sut.AddMultipleProductMovement(DateTime.Today, "CMD3", new List<ProductMovement>()
            {
                new ProductMovement(secondId, -3 ),
                new ProductMovement(thirdId, -1 ),
            });
            
            //Act
            var variations = sut.GetStockVariationBetweenDates(DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1));

            //Assert
            Assert.AreEqual(3,variations.Count);
            Assert.IsTrue(variations.Any( v => v.EanId == firstId && v.Quantity == 0));
            Assert.IsTrue(variations.Any( v => v.EanId == secondId && v.Quantity == 0));
            Assert.IsTrue(variations.Any( v => v.EanId == thirdId && v.Quantity == 0));
        }


        [TestMethod]
        public void ReturnVariationsOfStock()
        {
            
            //Arrange
            
            var sut = new Stock();
            var firstId = new EanId("EAN00001");
            var secondId = new EanId("EAN00002");
            var thirdId = new EanId("EAN00003");
            sut.AddInventoryMovement(DateTime.Today.AddDays(-7), firstId, 2);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), secondId, 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-4), thirdId, 8);
            sut.AddProductMovement(DateTime.Today.AddDays(-3), "CMD", firstId, 2);
            sut.AddProductMovement(DateTime.Today, "CMD2", firstId, 7);
            sut.AddMultipleProductMovement(DateTime.Today.AddDays(1), "CMD3", new List<ProductMovement>()
            {
                new ProductMovement(secondId, -3 ),
                new ProductMovement(thirdId, -1 ),
            });
            
            //Act
            var variations = sut.GetStockVariationBetweenDates(DateTime.Today.AddDays(-5), DateTime.Today);

            //Assert
            Assert.AreEqual(3,variations.Count);
            Assert.IsTrue(variations.Any( v => v.EanId == firstId && v.Quantity == 9));
            Assert.IsTrue(variations.Any( v => v.EanId == secondId && v.Quantity == 3));
            Assert.IsTrue(variations.Any( v => v.EanId == thirdId && v.Quantity == 8));
        }

        [TestMethod]
        public void ThrowExceptionWhenStartIsAfterEnd()
        {
            //Arrange
            var sut = new Stock();
            //Act
            //Assert
            var ex = Assert.ThrowsException<InvalidIntervalException>(() => sut.GetStockVariationBetweenDates(DateTime.Today.AddDays(1), DateTime.Today.AddDays(-1)));
            Assert.AreEqual("End of interval must follow start of interval",ex.Message );
        }


    }
}