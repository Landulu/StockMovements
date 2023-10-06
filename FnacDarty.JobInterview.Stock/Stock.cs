using System;
using System.Collections.Generic;
using System.Linq;
using FnacDarty.JobInterview.Stock.DomainExceptions;

namespace FnacDarty.JobInterview.Stock
{
    public class Stock
    {
        public IList<Product> Products { get; private set; } = new List<Product>();
        public IList<Movement> MovementHistory { get; private set; } = new List<Movement>();
        
        private const string InventoryLabel = "Inventaire";

        public Stock AddMultipleProductMovement(DateTime date, string movementLabel, IList<ProductMovement> productMovements)
        {
            if (productMovements.Any(p => IsPriorToProductInventory(date, p.ProductId)))
            {
                throw new InvalidMovementException(
                    "Impossible to create movement on a product prior to an existing inventory movement");
            }
            
            if (!productMovements.Any())
            {
                throw new InvalidMovementException(
                    "Impossible to create a movement about no product");
            }

            foreach (var productMovement in productMovements)
            {
                var existingProduct = Products.FirstOrDefault(p => p.EanId.Equals(productMovement.ProductId));
                if (existingProduct is null)
                {
                    Products.Add(new Product(productMovement.ProductId, productMovement.Quantity));
                }
                else
                {
                    existingProduct.AddQuantity(productMovement.Quantity);
                }
            }
            MovementHistory.Add(new Movement(MovementType.Standard, movementLabel, productMovements, date));
            return this;
        }
        
        public Stock AddProductMovement(DateTime date, string movementLabel, EanId productId, int quantity)
        {
            if (IsPriorToProductInventory(date, productId))
            {
                throw new InvalidMovementException(
                    "Impossible to create movement on a product prior to an existing inventory movement");
            }
            var existingProduct = Products.FirstOrDefault(p => p.EanId.Equals(productId));
            if (existingProduct is null)
            {
                Products.Add(new Product(productId, quantity));
            }
            else
            {
                existingProduct.AddQuantity(quantity);
            }
            MovementHistory.Add(new Movement(
                MovementType.Standard, 
                movementLabel, 
                new List<ProductMovement>{new ProductMovement(productId, quantity)}, 
                date));
            return this;
        }
        
        public Stock AddInventoryMovement(DateTime date, EanId productId, int quantity)
        {
            if (quantity < 0)
            {
                throw new InvalidMovementException(
                    "Impossible to create an inventory movement with an negative quantity");
            }
            if (IsPriorToProductInventory(date, productId))
            {
                throw new InvalidMovementException(
                    "Impossible to create movement on a product prior to an existing inventory movement");
            }
            
            var existingProduct = Products.FirstOrDefault(p => p.EanId.Equals(productId));
            if (existingProduct is null)
            {
                Products.Add(new Product(productId, quantity));
            }
            else
            {
                existingProduct.SetQuantity(quantity);
            }
            
            MovementHistory.Add(new Movement(
                MovementType.Inventory, 
                InventoryLabel, 
                new List<ProductMovement>{new ProductMovement(productId, quantity)}, 
                date));
            
            return this;
        }

        public IList<Product> GetStockAtFixedDate(DateTime date)
        {
            var result = new List<Product>(); 
            foreach (var movement in MovementHistory
                         .Where(m => m.Date <= date)
                         .OrderBy(m => m.Date))
                {
                    foreach (var productMovement in movement.ProductMovements)
                    {
                        var existingProduct = result.FirstOrDefault(p => p.EanId == productMovement.ProductId);
                        if (existingProduct is null)
                        {
                            result.Add(new Product(productMovement.ProductId, productMovement.Quantity));
                        }
                        else
                        {
                            if (movement.Type is MovementType.Inventory)
                            {
                                existingProduct.SetQuantity(productMovement.Quantity);
                            }
                            else
                            {
                                existingProduct.AddQuantity(productMovement.Quantity);
                            }
                        }
                    }
                }
            return result;
        }

        public IList<Product> GetStockVariationBetweenDates(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new InvalidIntervalException("End of interval must follow start of interval");
            }
            
            var variations = new List<Product>();
            var startStock = GetStockAtFixedDate(start);
            var endStock = GetStockAtFixedDate(end);
            
            foreach (var product in endStock)
            {
                var startStockProduct = startStock.FirstOrDefault(p => p.EanId == product.EanId);
                if (startStockProduct != null)
                {
                    product.AddQuantity(-startStockProduct.Quantity);
                }
                variations.Add(product);
            }

            return variations;
        }

        private bool IsPriorToProductInventory(DateTime date, EanId productId)
        {
            return MovementHistory.Any(m =>
                m.ProductMovements.Any(p => p.ProductId == productId) &&
                m.Type is MovementType.Inventory &&
                m.Date > date);
        }
    }
}
