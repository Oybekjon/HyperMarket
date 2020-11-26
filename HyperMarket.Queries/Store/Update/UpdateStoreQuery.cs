using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store.Update
{
    public class UpdateStoreQuery : StoreQuery, IQuery<UpdateStoreResult>
    {
        public long? StoreId { get; set; }
    }
}
