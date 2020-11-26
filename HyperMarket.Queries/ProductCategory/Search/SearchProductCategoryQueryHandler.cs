using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ProductCategory.Search
{
    public class SearchProductCategoryQueryHandler : BusinessLogicQueryHandler<SearchProductCategoryQuery, SearchProductCategoryResult>
    {
        public override Task<SearchProductCategoryResult> Handle(SearchProductCategoryQuery input)
        {
        
            throw new NotImplementedException();
        }
    }
}
