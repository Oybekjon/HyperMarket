using System;
using System.Collections.Generic;

namespace HyperMarket.DomainObjects
{
    public class Store : DatedEntity
    {
        private ICollection<UserStore> _Users;
        private ICollection<ProductStock> _ProductStocks;
        public long StoreId { get; set; }
        public string Name { get; set; }
        public ICollection<UserStore> Users
        {
            get => _Users ??= new List<UserStore>();
            set => _Users = value;
        }
        public ICollection<ProductStock> ProductStocks
        {
            get => _ProductStocks ??= new List<ProductStock>();
            set => _ProductStocks = value;
        }
    }
}