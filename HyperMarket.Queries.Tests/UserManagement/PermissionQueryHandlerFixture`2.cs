using HyperMarket.Queries.Tests.DataHelpers;

namespace HyperMarket.Queries.Tests.UserManagement
{
    public class PermissionQueryHandlerFixture<TQuery, TResult> : UserDataFixture<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public string AddedPermission { get; }
        public string NotAddedPermission { get; }
        public string InvalidPermission { get; }

        public PermissionQueryHandlerFixture()
        {
            AddedPermission = HyperMarketClaims.DeliveryPerson;
            NotAddedPermission = HyperMarketClaims.ProductManager;
            InvalidPermission = "InvalidPermission";
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            Context.AddUserPermission(FullUserId, AddedPermission);
        }
    }
}
