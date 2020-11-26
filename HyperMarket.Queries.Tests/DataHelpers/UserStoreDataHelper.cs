using HyperMarket.Data.SqlServer;
using HyperMarket.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class UserStoreDataHelper
    {
        public static UserStoreFluentDataHelper AddUserStore(this MainContext context, long userId, long storeId)
        {
            context.UserStores.Add(new UserStore
            {
                UserId = userId,
                StoreId = storeId
            });

            context.SaveChanges();

            return new UserStoreFluentDataHelper(context, userId, storeId);
        }

        public class UserStoreFluentDataHelper
        {
            public MainContext Context { get; }
            public long UserId { get; }
            public long StoreId { get; }

            public UserStoreFluentDataHelper(MainContext context, long userId, long storeId)
            {
                Context = context;
                UserId = userId;
                StoreId = storeId;
            }

            public UserStoreFluentDataHelper AddStorePermission(string permission) {
                return AddStorePermissions(new[] { permission });
            }

            public UserStoreFluentDataHelper AddStorePermissions(IEnumerable<string> permissions)
            {
                permissions.ForEach(x => 
                Context.UserStorePermissions.Add(new UserStorePermission
                    {
                        Permission = x,
                        UserId = UserId,
                        StoreId = StoreId
                    })
                );
                Context.SaveChanges();
                return this;
            }
        }
    }
}
