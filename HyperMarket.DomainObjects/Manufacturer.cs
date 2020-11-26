namespace HyperMarket.DomainObjects
{
    public class Manufacturer
    {
        public long ManufacturerId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}