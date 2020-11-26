using HyperMarket.Queries.Store.Create;
using HyperMarket.Queries.Tests.DataHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.Store.Create
{
    public class CreateStoreQueryHandlerFixture : BaseDataFixture<CreateStoreQuery, CreateStoreResult>
    {
        public long StoreId { get; private set; }
        public long AddressId { get; private set; }
        public string StoreName { get; private set; }

        public string NewStoreName { get; }
        public string NewStoreDisplayName { get; }
        public string NewStoreDescription { get; }

        public CreateStoreQueryHandlerFixture()
        {
            NewStoreName =
            NewStoreDisplayName =
            NewStoreDescription =
                Guid.NewGuid().ToString();
        }

        public CreateStoreQuery GetQuery(
            string name = null,
            string displayName = null,
            string description = null,
            long? addressId = null,
            string streetAddress = null,
            string house = null,
            string appartment = null,
            string entrance = null,
            string floor = null,
            string city = null,
            string region = null,
            string postalCode = null)
        {
            return new CreateStoreQuery
            {
                Name = name ?? NewStoreName,
                DisplayName = displayName ?? NewStoreDisplayName,
                Description = description ?? NewStoreDescription,
                AddressId = addressId,
                StreetAddress = streetAddress,
                House = house,
                Appartment = appartment,
                Entrance = entrance,
                Floor = floor,
                City = city,
                Region = region,
                PostalCode = postalCode
            };
        }

        protected override void InitDatabase()
        {
            Context.AddStore(out var storeId, x =>
            {
                StoreName = x.Name;
                x.Description = x.Name;
                x.DisplayName = x.Name;
            });

            Context.AddAddress(out var addressId, x =>
            {
                x.City = "Tashkent";
                x.StreetAddress = "Some street";
                x.House = "19";
                x.Appartment = "99";
            });

            StoreId = storeId;
            AddressId = addressId;
        }
    }
}
