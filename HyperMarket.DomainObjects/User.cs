using System;
using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class User : DatedEntity
    {
        private ICollection<Cart> _Carts;
        private ICollection<UserAddress> _Addresses;
        private ICollection<Order> _Orders;
        private ICollection<Order> _OrdersToDeliver;
        private ICollection<UserPermission> _UserPermissions;
        private ICollection<UserStore> _UserStores;

        public long UserId { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailValidationCode { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string FbId { get; set; }
        public string GoogleId { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserPermission> UserPermissions
        {
            get => _UserPermissions ??= new List<UserPermission>();
            set => _UserPermissions = value;
        }

        public ICollection<UserAddress> Addresses
        {
            get => _Addresses ??= new List<UserAddress>();
            set => _Addresses = value;
        }

        public ICollection<Cart> Carts
        {
            get => _Carts ??= new List<Cart>();
            set => _Carts = value;
        }

        public ICollection<Order> Orders
        {
            get => _Orders ??= new List<Order>();
            set => _Orders = value;
        }

        public ICollection<Order> OrdersToDeliver
        {
            get => _OrdersToDeliver ??= new List<Order>();
            set => _OrdersToDeliver = value;
        }
        
        public ICollection<UserStore> UserStores
        {
            get => _UserStores ??= new List<UserStore>();
            set => _UserStores = value;
        }
    }
}
