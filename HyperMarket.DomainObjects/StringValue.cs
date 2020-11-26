namespace HyperMarket.DomainObjects
{
    public class StringValue
    {
        public long StringValueId { get; set; }
        public string Value { get; set; }
        public long StringPropertyId { get; set; }

        public StringProperty StringProperty { get; set; }
    }
}