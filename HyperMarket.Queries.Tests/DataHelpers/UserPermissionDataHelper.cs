using HyperMarket.Data.SqlServer;
using HyperMarket.DomainObjects;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class UserPermissionDataHelper
    {
        public static UserPermissionFluentDataHelper AddUserPermission(this MainContext context, long userId, params string[] permissions)
        {
            foreach (var item in permissions)
            {
                context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    Permission = item
                });
            }
            context.SaveChanges();
            return new UserPermissionFluentDataHelper(context, userId);
        }

        public class UserPermissionFluentDataHelper
        {
            public MainContext Context { get; }
            public long ParentId { get; }

            public UserPermissionFluentDataHelper(MainContext context, long parentId)
            {
                Context = context;
                ParentId = parentId;
            }
        }
    }
}
