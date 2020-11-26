using HyperMarket.Queries.ViewModels;
using HyperMarket.Queries.ViewModels.Store;
using System.Collections.Generic;

namespace HyperMarket.Queries.Store.GetList
{
    public class StoreListResult : ListViewModel<StoreViewModel>
    {
        public StoreListResult()
        {
        }

        public StoreListResult(List<StoreViewModel> data)
            : base(data)
        {
        }

        public StoreListResult(List<StoreViewModel> data, int totalRecords)
            : base(data, totalRecords)
        {
        }

        public StoreListResult(IEnumerable<StoreViewModel> data, int totalRecords)
            : base(data, totalRecords)
        {

        }
    }
}