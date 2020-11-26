using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ViewModels.ProductCategories
{
    public class ProductCategoryViewModel
    {
        public long? ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string NameRu { get; set; }
        public string NameUz { get; set; }
        public string NameTj { get; set; }
        public long? ParentProductCategoryId { get; set; }
    }
}
