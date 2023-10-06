using System;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class AddInventoryMovementShould
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
            sut.AddInventoryMovement(movementDate, firstProductId, 1);
            sut.AddInventoryMovement(movementDate, secondProductId, 3);
            
            //Assert
            Assert.AreEqual(1, sut.Products.Count(p => p.EanId.Equals(firstProductId)));
            Assert.AreEqual(1, sut.Products.Count(p => p.EanId.Equals(secondProductId)));
            Assert.IsTrue(sut.Products.Count == 2);
            Assert.AreEqual(1, sut.Products.First(p => p.EanId == firstProductId).Quantity);
            Assert.AreEqual(3, sut.Products.First(p => p.EanId == secondProductId).Quantity);
        }

        [TestMethod]
        public void SetProductWithNewQuantityIfAlreadyExists()
        {
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var productId = new EanId("EAN00000");
            
            //Act
            sut.AddInventoryMovement(movementDate.AddDays(-1), productId, 2);
            sut.AddInventoryMovement(movementDate, productId, 3);
            
            //Assert
            Assert.AreEqual(1, sut.Products.Count());
            Assert.AreEqual(productId, sut.Products.First().EanId);
            Assert.AreEqual(3, sut.Products.First().Quantity);
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
            var ex = Assert.ThrowsException<InvalidMovementException>(() => sut.AddInventoryMovement(movementDate.AddDays(-1), productId, 3));
            Assert.AreEqual("Impossible to create movement on a product prior to an existing inventory movement",ex.Message );
        }

        [TestMethod]
        public void ThrowExceptionWhenProductQuantityIsNegative()
        {
            //Arrange
            var sut = new Stock();
            var productId = new EanId("EAN00000");
            
            //Act
            //Assert
            var ex = Assert.ThrowsException<InvalidMovementException>(() => sut.AddInventoryMovement(DateTime.Now, productId, -1));
            Assert.AreEqual("Impossible to create an inventory movement with an negative quantity",ex.Message );
        }
    }
}