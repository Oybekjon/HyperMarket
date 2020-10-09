using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class UserStore
    {
        private ICollection<UserPermission> _Permissions;
        public long UserId { get; set; }
        public long StoreId { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
        public ICollection<UserPermission> Permissions
        {
            get => _Permissions ??= new List<UserPermission>();
            set => _Permissions = value;
        }
    }
}