using System;
using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class User : DatedEntity
    {
        private ICollection<UserStore> _Stores;
        private ICollection<Cart> _Carts;
        private ICollection<UserPermission> _Permissions;
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FbId { get; set; }
        public string GoogleId { get; set; }

        public ICollection<UserStore> Stores
        {
            get => _Stores ??= new List<UserStore>();
            set => _Stores = value;
        }

        public ICollection<Cart> Carts
        {
            get => _Carts ??= new List<Cart>();
            set => _Carts = value;
        }

        public ICollection<UserPermission> Permissions
        {
            get => _Permissions ??= new List<UserPermission>();
            set => _Permissions = value;
        }
    }
}
