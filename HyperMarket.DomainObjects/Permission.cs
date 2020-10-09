using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class Permission : DatedEntity
    {
        private ICollection<UserPermission> _Users;
        
        public long PermissionId { get; set; }

        public ICollection<UserPermission> Users
        {
            get => _Users ??= new List<UserPermission>();
            set => _Users = value;
        }
    }
}