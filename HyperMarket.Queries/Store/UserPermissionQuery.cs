using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store
{
    public class UserPermissionQuery
    {
        private List<string> _Permissions;
        public long UserId { get; set; }
        public List<string> Permissions
        {
            get => _Permissions ??= new List<string>();
            set => _Permissions = value;
        }
        public long StoreId { get; set; }
    }
}
