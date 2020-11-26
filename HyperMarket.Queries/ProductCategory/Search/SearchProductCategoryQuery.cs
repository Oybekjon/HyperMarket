using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ProductCategory.Search
{
    public class SearchProductCategoryQuery : IQuery<SearchProductCategoryResult>
    {
        public long? ProductCategoryId { get; set; }
        public long? ParentCategoryId { get; set; }
        public ParentalInclusion ParentalInclusion { get; set; }
        public int? ParentLevel { get; set; }
        public ChildInclusion ChildInclusion { get; set; }
        public int? ChildrenLevel { get; set; }

        public string Query { get; set; }
        public string Language { get; set; }
    }
}
