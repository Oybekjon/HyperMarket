using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.UserManagement.GetPermissions
{
    public class GetPermissionQuery : IQuery<GetPermissionResult>
    {
        public long UserId { get; set; }
    }
}
