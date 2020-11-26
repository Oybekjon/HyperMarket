using HyperMarket.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store.GetList
{
    public class StoreListQuery : ListQueryViewModel<StoreListResult>
    {
        public long? StoreId { get; set; }
    }
}
