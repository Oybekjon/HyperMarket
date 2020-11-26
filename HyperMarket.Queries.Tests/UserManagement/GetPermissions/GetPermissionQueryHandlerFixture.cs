using HyperMarket.Queries.UserManagement.GetPermissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.UserManagement.GetPermissions
{
    public class GetPermissionQueryHandlerFixture : PermissionQueryHandlerFixture<GetPermissionQuery, GetPermissionResult>
    {
        public GetPermissionQuery GetQuery(long? userId = null)
        {
            return new GetPermissionQuery
            {
                UserId = userId ?? FullUserId
            };
        }
    }
}
