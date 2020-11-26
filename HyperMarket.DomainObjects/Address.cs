using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class Address
    {
        private ICollection<Order> _Orders;
        private ICollection<Store> _Stores;
        private ICollection<UserAddress> _Users;

        public long AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string House { get; set; }
        public string Appartment { get; set; }
        public string Entrance { get; set; }
        public string Floor { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }

        public ICollection<Order> Orders
        {
            get => _Orders ??= new List<Order>();
            set => _Orders = value;
        }

        public ICollection<Store> Stores
        {
            get => _Stores ??= new List<Store>();
            set => _Stores = value;
        }

        public ICollection<UserAddress> Users
        {
            get => _Users ??= new List<UserAddress>();
            set => _Users = value;
        }
    }
}