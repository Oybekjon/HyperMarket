using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store
{
    public class StoreQuery
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public long? AddressId { get; set; }

        public string StreetAddress { get; set; }
        public string House { get; set; }
        public string Appartment { get; set; }
        public string Entrance { get; set; }
        public string Floor { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
    }
}
