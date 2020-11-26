using HyperMarket.Data.SqlServer;
using HyperMarket.DomainObjects;
using System;

namespace HyperMarket.Queries.Tests.DataHelpers
{
    public static class AddressDataHelper
    {
        public static AddressFluentDataHelper AddAddress(
            this MainContext context,
            out long addressId,
            Action<Address> setupAction = null)
        {
            var address = new Address();
            setupAction?.Invoke(address);
            context.Addresses.Add(address);
            context.SaveChanges();
            addressId = address.AddressId;
            return new AddressFluentDataHelper(context, addressId);
        }

        public class AddressFluentDataHelper
        {
            public MainContext Context { get; }
            public long ParentId { get; }

            public AddressFluentDataHelper(MainContext context, long parentId)
            {
                Context = context;
                ParentId = parentId;
            }
        }
    }
}
