using HyperMarket.Queries.UserManagement.AssignPermissions;
using System.Collections.Generic;

namespace HyperMarket.Queries.Tests.UserManagement.AssignPermissions
{
    public class AssignPermissionQueryHandlerFixture : PermissionQueryHandlerFixture<AssignPermissionQuery, AssignPermissionResult>
    {
        public AssignPermissionQuery GetQuery(
            long? userId = null,
            string permission = null
        )
        {
            return new AssignPermissionQuery
            {
                UserId = userId ?? FullUserId,
                Permissions = new List<string> { permission ?? NotAddedPermission }
            };
        }
    }
}
