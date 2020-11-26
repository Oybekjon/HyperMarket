namespace HyperMarket.DomainObjects
{
    public class StringPropertyValue
    {
        public long StringPropertyValueId { get; set; }
        public string Value { get; set; }
        public long? StringValueId { get; set; }
        public StringValue StringValue { get; set; }
        public ProductStock ProductStock { get; set; }
    }
}