namespace HyperMarket.DomainObjects
{
    public class DoubleValue
    {
        public long DoubleValueId { get; set; }
        public double? Value { get; set; }
        public long DoublePropertyId { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        public DoubleProperty DoubleProperty { get; set; }
    }
}