using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class UserStore
    {
        private ICollection<UserStorePermission> _UserStorePermissions;
        public long UserId { get; set; }
        public long StoreId { get; set; }
        public User User { get; set; }
        public Store Store { get; set; }

        public ICollection<UserStorePermission> UserStorePermissions
        {
            get => _UserStorePermissions ??= new List<UserStorePermission>();
            set => _UserStorePermissions = value;
        }
    }
}