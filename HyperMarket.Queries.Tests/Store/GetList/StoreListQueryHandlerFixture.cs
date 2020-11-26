using HyperMarket.Queries.Store.GetList;
using HyperMarket.Queries.Tests.DataHelpers;

namespace HyperMarket.Queries.Tests.Store.GetList
{
    public class StoreListQueryHandlerFixture : BaseDataFixture<StoreListQuery, StoreListResult>
    {
        public long StoreId { get; private set; }
        public long SecondStoreId { get; private set; }

        protected override void InitDatabase()
        {
            Context.AddStore(out var storeId);
            Context.AddStore(out var secondStoreId);

            StoreId = storeId;
            SecondStoreId = secondStoreId;
        }
    }
}
