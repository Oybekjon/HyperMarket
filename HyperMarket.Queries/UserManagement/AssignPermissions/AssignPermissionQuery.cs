using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.UserManagement.AssignPermissions
{
    public class AssignPermissionQuery : IQuery<AssignPermissionResult>
    {
        private List<string> permissions;

        public List<string> Permissions
        {
            get => permissions ??= new List<string>();
            set => permissions = value;
        }

        public long UserId { get; set; }
    }
}
