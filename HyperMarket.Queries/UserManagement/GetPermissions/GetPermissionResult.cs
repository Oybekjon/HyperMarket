using System.Collections.Generic;

namespace HyperMarket.Queries.UserManagement.GetPermissions
{
    public class GetPermissionResult
    {
        public IReadOnlyCollection<string> Permissions { get; }
        public GetPermissionResult(IReadOnlyCollection<string> permissions)
        {
            Permissions = permissions;
        }
    }
}