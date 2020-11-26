namespace HyperMarket.DomainObjects
{
    public class ProductDoubleProperty
    {
        public long ProductId { get; set; }
        public long DoublePropertyId { get; set; }
        public Product Product { get; set; }
        public DoubleProperty DoubleProperty { get; set; }
    }
}