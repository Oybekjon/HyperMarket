using HyperMarket.Queries.Store.Update;
using HyperMarket.Queries.Tests.DataHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests.Store.Update
{
    public class UpdateStoreQueryHandlerFixture : BaseDataFixture<UpdateStoreQuery, UpdateStoreResult>
    {
        public long StoreId { get; private set; }
        public long AddressId { get; private set; }
        public string StoreName { get; private set; }

        public long NoAddressStoreId { get; private set; }
        public string NoAddressStoreName { get; private set; }

        public string NewStoreName { get; }
        public string NewStoreDisplayName { get; }
        public string NewStoreDescription { get; }

        public string City { get; }
        public string StreetAddress { get; }
        public string House { get; }
        public string Appartment { get; }

        public string NewCity { get; }
        public string NewStreetAddress { get; }
        public string NewHouse { get; }
        public string NewAppartment { get; }

        public UpdateStoreQueryHandlerFixture()
        {
            NewStoreName =
                NewStoreDisplayName =
                NewStoreDescription =
                    Guid.NewGuid().ToString();

            City = "Tashkent";
            StreetAddress = "Some street";
            House = "19";
            Appartment = "99";

            NewCity = "Samarkand";
            NewStreetAddress = "Some street 2";
            NewHouse = "29";
            NewAppartment = "19";
        }

        public UpdateStoreQuery GetQuery(
            long storeId,
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
            return new UpdateStoreQuery { 
                StoreId = storeId,
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
            Context.AddAddress(out var addressId, x =>
            {
                x.City = City;
                x.StreetAddress = StreetAddress;
                x.House = House;
                x.Appartment = Appartment;
            });

            Context.AddStore(out var storeId, x =>
            {
                StoreName = x.Name;
                x.Description = x.Name;
                x.DisplayName = x.Name;
                x.AddressId = addressId;
            });

            Context.AddStore(out var noAddressStoreId, x =>
            {
                NoAddressStoreName = x.Name;
                x.Description = x.Name;
                x.DisplayName = x.Name;
            });

            NoAddressStoreId = noAddressStoreId;
            StoreId = storeId;
            AddressId = addressId;
        }
    }
}
