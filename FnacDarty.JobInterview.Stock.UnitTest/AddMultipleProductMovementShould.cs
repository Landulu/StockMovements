using System;
using System.Collections.Generic;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class AddMultipleProductMovementShould
    {

        [TestMethod]
        public void ThrowExceptionIfNoProducts()
        {
            //Arrange
            var sut = new Stock();
            //Act
            //Assert
            var ex = Assert.ThrowsException<InvalidMovementException>(() => 
                sut.AddMultipleProductMovement(DateTime.Today, "CMD", new List<ProductMovement>()));

            Assert.AreEqual("Impossible to create a movement about no product",ex.Message );
        }


        [TestMethod]
        public void CreateNewProductIfNew()
        {
            //arrange
            var sut = new Stock();
            var productId1 = new EanId("EAN00001");
            var productId2 = new EanId("EAN00002");
            var productId3 = new EanId("EAN00003");
            var productMovements = new List<ProductMovement>()
            {
                new ProductMovement(productId1, 1),
                new ProductMovement(productId2, 2),
                new ProductMovement(productId3, -3)
            };
            
            //act
            sut.AddMultipleProductMovement(DateTime.Today, "MOV", productMovements);
            
            //Assert
            Assert.IsTrue(sut.Products.Count == 3);
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId1 && p.Quantity == 1));
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId2 && p.Quantity == 2));
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId3 && p.Quantity == -3));
        }

        [TestMethod]
        public void UpdateProductQuantitiesIfProductAlreadyExist()
        {
            //arrange
            var sut = new Stock();
            var productId1 = new EanId("EAN00001");
            var productId2 = new EanId("EAN00002");
            var productId3 = new EanId("EAN00003");
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), productId1, 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), productId2, 3);
            sut.AddInventoryMovement(DateTime.Today.AddDays(-1), productId3, 3);
            var productMovements = new List<ProductMovement>()
            {
                new ProductMovement(productId1, 1),
                new ProductMovement(productId2, 2),
                new ProductMovement(productId3, -3)
            };
            
            //act
            sut.AddMultipleProductMovement(DateTime.Today, "MOV", productMovements);
            
            //Assert
            Assert.IsTrue(sut.Products.Count == 3);
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId1 && p.Quantity == 4));
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId2 && p.Quantity == 5));
            Assert.IsTrue(sut.Products.Any(p => p.EanId == productId3 && p.Quantity == 0));
        }

        [TestMethod]
        public void AddMovementEntries()
        {
            //arrange
            var sut = new Stock();
            var productId1 = new EanId("EAN00001");
            var productId2 = new EanId("EAN00002");
            var productId3 = new EanId("EAN00003");
            var firstProductMovements = new List<ProductMovement>()
            {
                new ProductMovement(productId1, 1),
                new ProductMovement(productId2, 2),
            };
            var secondProductMovements = new List<ProductMovement>()
            {
                new ProductMovement(productId3, -3)
            };
            
            //act
            sut.AddMultipleProductMovement(DateTime.Today.AddDays(-2), "CMD1", firstProductMovements);
            sut.AddMultipleProductMovement(DateTime.Today, "CMD2", secondProductMovements);
            
            //assert
            Assert.IsTrue(sut.MovementHistory[0].Label == "CMD1");
            Assert.IsTrue(sut.MovementHistory[0].ProductMovements.Count == 2);
            Assert.IsTrue(sut.MovementHistory[1].Label == "CMD2");
            Assert.IsTrue(sut.MovementHistory[1].ProductMovements.Count == 1);
        }
        
        [TestMethod]
        public void ThrowExceptionAndNotChangeProductsIfDateIsPriorToOnlyOneOfTheMovementProducts()
        {
            //Arrange
            var sut = new Stock();
            var movementDate = DateTime.Now;
            var productId1 = new EanId("EAN00001");
            var productId2 = new EanId("EAN00002");
            var productId3 = new EanId("EAN00003");
            var productMovements = new List<ProductMovement>()
            {
                new ProductMovement(productId1, 1),
                new ProductMovement(productId2, 3),
                new ProductMovement(productId3, -5),
            };
            //Act
            sut.AddInventoryMovement(movementDate, productId1, 2);
            sut.AddInventoryMovement(movementDate.AddDays(-10), productId2, 4);
            //Assert
            var ex = Assert.ThrowsException<InvalidMovementException>(() => sut.AddMultipleProductMovement(movementDate.AddDays(-1), "CMD", productMovements));
            Assert.AreEqual("Impossible to create movement on a product prior to an existing inventory movement",ex.Message );
            Assert.AreEqual(2, sut.Products.Count );
            Assert.AreEqual(2, sut.MovementHistory.Count );
            Assert.AreEqual(2, sut.Products.First(p => p.EanId== productId1).Quantity );
            Assert.AreEqual(4, sut.Products.First(p => p.EanId== productId2).Quantity );
            Assert.IsFalse(sut.Products.Any(p => p.EanId.Equals(productId3)));
        }
    }
}