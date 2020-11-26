namespace HyperMarket.DomainObjects
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public double Quantity { get; set; }
        public long ProductStockId { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public ProductStock ProductStock { get; set; }
        public Order Order { get; set; }
    }
}