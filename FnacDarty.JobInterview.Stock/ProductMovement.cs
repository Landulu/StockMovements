namespace FnacDarty.JobInterview.Stock
{
    public class ProductMovement
    {
        public ProductMovement(EanId productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public EanId ProductId { get; private set; }
        public int Quantity { get; private set; }
        
    }
}