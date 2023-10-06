using System;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class AddProductMovementShould
    {
        
        [TestMethod]
        public void CreateProductInStockIfNotAlreadyExists()
        {
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var firstProductId = new EanId("EAN12345");
            var secondProductId = new EanId("EAN54321");
            
            //Act
            sut.AddProductMovement(movementDate, "CMD1", firstProductId, 2);
            sut.AddProductMovement(movementDate, "CMD2", secondProductId, -5);
            
            //Assert
            Assert.AreEqual(1, sut.Products.Count(p => p.EanId.Equals(firstProductId)));
            Assert.AreEqual(1, sut.Products.Count(p => p.EanId.Equals(secondProductId)));
            Assert.IsTrue(sut.Products.Count == 2);
            Assert.AreEqual(2, sut.Products.First(p => p.EanId == firstProductId).Quantity);
            Assert.AreEqual(-5, sut.Products.First(p => p.EanId == secondProductId).Quantity);
        }
        
        
        [TestMethod]
        public void SetProductQuantityAdditivelyIfAlreadyExists()
        {
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var productId = new EanId("EAN12345");
            
            //Act
            sut.AddProductMovement(movementDate, "CMD1", productId, 2);
            sut.AddProductMovement(movementDate, "CMD2", productId, -5);
            
            //Assert
            Assert.IsTrue(sut.Products.Count == 1);
            Assert.AreEqual(-3, sut.Products.First(p => p.EanId == productId).Quantity);
        }

        [TestMethod]
        public void AddMovementEntries()
        {
            
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var firstProductId = new EanId("EAN12345");
            var secondProductId = new EanId("EAN54321");
            
            //Act
            sut.AddProductMovement(movementDate.AddDays(-2), "CMD1", firstProductId, 2);
            sut.AddProductMovement(movementDate.AddDays(-1), "CMD2", secondProductId, -5);
            sut.AddProductMovement(movementDate, "CMD3", firstProductId, 4);

            
            //Assert
            Assert.IsTrue(sut.MovementHistory.Any(m => 
                m.Label == "CMD1" 
                && m.ProductMovements.Count == 1 
                && m.Date == movementDate.AddDays(-2) 
                && m.ProductMovements.First().Quantity == 2 && m.ProductMovements.First().ProductId == firstProductId));
            
            Assert.IsTrue(sut.MovementHistory.Any(m => 
                m.Label == "CMD2" 
                && m.ProductMovements.Count == 1 
                && m.Date == movementDate.AddDays(-1) 
                && m.ProductMovements.First().Quantity == -5 && m.ProductMovements.First().ProductId == secondProductId));
            
            Assert.IsTrue(sut.MovementHistory.Any(m => 
                m.Label == "CMD3" 
                && m.ProductMovements.Count == 1 
                && m.Date == movementDate 
                && m.ProductMovements.First().Quantity == 4 && m.ProductMovements.First().ProductId == firstProductId));

        }
        
        
        [TestMethod]
        public void NotBePossiblePriorToAnotherInventoryMovementForSpecificProduct()
        {
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var productId = new EanId("EAN00000");
            
            //Act
            sut.AddInventoryMovement(movementDate, productId, 2);
            //Assert
            var ex = Assert.ThrowsException<InvalidMovementException>(() => sut.AddProductMovement(movementDate.AddDays(-1), "CMD", productId, 3));
            Assert.AreEqual("Impossible to create movement on a product prior to an existing inventory movement",ex.Message );
        }
        
    }
}