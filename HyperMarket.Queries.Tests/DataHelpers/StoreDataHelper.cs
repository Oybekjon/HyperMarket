using HyperMarket.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using HyperMarket.DomainObjects;
using HMStore = HyperMarket.DomainObjects.Store;
using static HyperMarket.Queries.Tests.DataHelpers.UserStoreDataHelper;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class StoreDataHelper
    {
        public static StoreFluentDataHelper AddStore(
            this MainContext context, 
            out long storeId, 
            Action<HMStore> setupAction = null
        )
        {
            var store = new HMStore
            {
                DateCreated = DateTime.UtcNow,
                Name = Guid.NewGuid().ToString()
            };

            setupAction?.Invoke(store);
            context.Stores.Add(store);
            context.SaveChanges();
            storeId = store.StoreId;
            return new StoreFluentDataHelper(context, storeId);
        }

        public static StoreFluentDataHelper AddStore(this MainContext context)
        {
            return context.AddStore(out _);
        }

        public class StoreFluentDataHelper
        {
            public MainContext Context { get; }
            public long ParentId { get; }

            public StoreFluentDataHelper(MainContext context, long parentId)
            {
                Context = context;
                ParentId = parentId;
            }

            public UserStoreFluentDataHelper AddUserStore(long userId)
            {
                return Context.AddUserStore(userId, ParentId);
            }
        }
    }
}
