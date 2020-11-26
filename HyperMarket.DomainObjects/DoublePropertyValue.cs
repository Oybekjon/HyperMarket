namespace HyperMarket.DomainObjects
{
    public class DoublePropertyValue
    {
        public long DoublePropertyValueId { get; set; }
        public double? Value { get; set; }
        public long? DoubleValueId { get; set; }
        
        public DoubleValue DoubleValue { get; set; }
        public ProductStock ProductStock { get; set; }
    }
}