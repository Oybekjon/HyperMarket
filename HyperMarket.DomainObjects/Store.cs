using System;
using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class Store : DatedEntity
    {
        private ICollection<ProductStock> _ProductStocks;
        private ICollection<UserStore> _UserStores;
        public long StoreId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public long? AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<ProductStock> ProductStocks
        {
            get => _ProductStocks ??= new List<ProductStock>();
            set => _ProductStocks = value;
        }

        public ICollection<UserStore> UserStores
        {
            get => _UserStores ??= new List<UserStore>();
            set => _UserStores = value;
        }
    }
}