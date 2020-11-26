using HyperMarket.Queries.Store;
using HyperMarket.Queries.Tests.DataHelpers;
using HyperMarket.Queries.Tests.User;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace HyperMarket.Queries.Tests.Store
{
    public abstract class UserPermissionQueryHandlerFixture<TQuery, TResult> : UserDataFixture<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public string NotAddedClaim { get; private set; }
        public IReadOnlyCollection<string> AddedClaims { get; private set; }
        public long StoreId { get; private set; }

        public T GetQuery<T>(
            long? userId = null,
            List<string> permissions = null,
            long? storeId = null) where T : UserPermissionQuery, new()
        {
            return new T
            {
                UserId = userId ?? FullUserId,
                Permissions = permissions,
                StoreId = storeId ?? StoreId
            };
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            var claims = HyperMarketClaims.StoreClaims();
            NotAddedClaim = claims.First();
            AddedClaims = claims.Skip(1).ToImmutableArray();

            var dataHelper = Context
                .AddStore(out var storeId, x =>
                {
                    x.Name =
                    x.DisplayName =
                    x.Description =
                        Guid.NewGuid().ToString();
                })
                .AddUserStore(FullUserId)
                .AddStorePermissions(AddedClaims);

            StoreId = storeId;
        }
    }
}
