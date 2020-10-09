namespace HyperMarket.DomainObjects
{
    public class ProductCategory
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public long? ParentProductCategoryId { get; set; }
        public ProductCategory ParentProductCategory { get; set; }
    }
}