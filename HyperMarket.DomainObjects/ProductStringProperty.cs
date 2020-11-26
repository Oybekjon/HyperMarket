namespace HyperMarket.DomainObjects
{
    public class ProductStringProperty
    {
        public long ProductId { get; set; }
        public long StringPropertyId { get; set; }
        public Product Product { get; set; }
        public StringProperty StringProperty { get; set; }
    }
}