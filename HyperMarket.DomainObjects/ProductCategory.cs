using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class ProductCategory
    {
        private ICollection<ProductCategory> _Children;
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string NameRu { get; set; }
        public string NameUz { get; set; }
        public string NameTj { get; set; }
        public long? ParentProductCategoryId { get; set; }
        public ProductCategory ParentProductCategory { get; set; }
        public ICollection<ProductCategory> Children
        {
            get => _Children ??= new List<ProductCategory>();
            set => _Children = value;
        }
    }
}