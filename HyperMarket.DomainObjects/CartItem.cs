namespace HyperMarket.DomainObjects
{
    public class CartItem : DatedEntity
    {
        public long CartItemId { get; set; }
        public long ProductStockId { get; set; }
        public double Quantity { get; set; }
        public long CartId { get; set; }
        public ProductStock ProductStock { get; set; }
        public Cart Cart { get; set; }
    }
}