using System;

namespace FnacDarty.JobInterview.Stock
{
    public class Product
    {
        public Product(EanId eanId, int quantity)
        {
            EanId = eanId;
            Quantity = quantity;
        }

        public EanId EanId { get; private set; }
        public int Quantity { get; private set; }

        public void SetQuantity(int productQuantity)
        {
            Quantity = productQuantity;
        }
        
        public void AddQuantity(int addedQuantity)
        {
            Quantity += addedQuantity;
        }
    }
}
