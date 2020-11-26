using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class UserAddress
    {
        public long UserId { get; set; }
        public long AddressId { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
    }
}
