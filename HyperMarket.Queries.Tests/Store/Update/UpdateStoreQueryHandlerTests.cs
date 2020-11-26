using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HyperMarket.Queries.Tests.Store.Update
{
    [TestClass]
    public class UpdateStoreQueryHandlerTests
    {
        [TestMethod]
        public async Task UpdateStore_Ok()
        {
            using var fixture = new UpdateStoreQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(
                fixture.NoAddressStoreId,
                name: fixture.NewStoreName,
                displayName: fixture.NewStoreDisplayName,
                description: fixture.NewStoreDescription
            );

            var result = await service.Handle(query);
            Assert.IsNotNull(result);

            var entity = fixture.Context.Stores.FirstOrDefault(x => x.StoreId == fixture.NoAddressStoreId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewStoreName, entity.Name);
            Assert.AreEqual(fixture.NewStoreDisplayName, entity.DisplayName);
            Assert.AreEqual(fixture.NewStoreDescription, entity.Description);
            Assert.IsNull(entity.AddressId);
        }

        [TestMethod]
        public async Task UpdateStore_WithAddress_Ok()
        {
            using var fixture = new UpdateStoreQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(
                fixture.NoAddressStoreId,
                name: fixture.NewStoreName,
                displayName: fixture.NewStoreDisplayName,
                description: fixture.NewStoreDescription,
                city: fixture.NewCity,
                streetAddress: fixture.NewStreetAddress,
                house: fixture.NewHouse,
                appartment: fixture.NewAppartment
            );

            var result = await service.Handle(query);
            Assert.IsNotNull(result);

            var entity = fixture.Context.Stores
                .Include(x => x.Address)
                .FirstOrDefault(x => x.StoreId == fixture.NoAddressStoreId);
            Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewStoreName, entity.Name);
            Assert.AreEqual(fixture.NewStoreDisplayName, entity.DisplayName);
            Assert.AreEqual(fixture.NewStoreDescription, entity.Description);
            Assert.IsNotNull(entity.AddressId);
            Assert.IsNotNull(entity.Address);
            Assert.AreEqual(entity.Address.City, fixture.NewCity);
            Assert.AreEqual(entity.Address.StreetAddress, fixture.NewStreetAddress);
            Assert.AreEqual(entity.Address.House, fixture.NewHouse);
            Assert.AreEqual(entity.Address.Appartment, fixture.NewAppartment);
        }

        [TestMethod]
        public async Task UpdateStore_WithAddress_StoreWithAddress_Ok()
        {
            using var fixture = new UpdateStoreQueryHandlerFixture();
            var service = fixture.CreateService();

            var oldAddress = fixture.Context.Stores
                .Where(x => x.StoreId == fixture.StoreId)
                .Select(x => new { 
                    x.StoreId,
                    x.AddressId
                }).First();

            var query = fixture.GetQuery(
                fixture.StoreId,
                name: fixture.NewStoreName,
                displayName: fixture.NewStoreDisplayName,
                description: fixture.NewStoreDescription,
                city: fixture.NewCity,
                streetAddress: fixture.NewStreetAddress,
                house: fixture.NewHouse,
                appartment: fixture.NewAppartment
            );

            var result = await service.Handle(query);
            Assert.IsNotNull(result);
            Assert.IsNotNull(oldAddress);
            Assert.IsNotNull(oldAddress.AddressId);

            var entity = fixture.Context.Stores
                .Include(x => x.Address)
                .FirstOrDefault(x => x.StoreId == fixture.StoreId);
            
                Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewStoreName, entity.Name);
            Assert.AreEqual(fixture.NewStoreDisplayName, entity.DisplayName);
            Assert.AreEqual(fixture.NewStoreDescription, entity.Description);
            Assert.IsNotNull(entity.AddressId);
            Assert.IsNotNull(entity.Address);
            Assert.AreEqual(entity.Address.City, fixture.NewCity);
            Assert.AreEqual(entity.Address.StreetAddress, fixture.NewStreetAddress);
            Assert.AreEqual(entity.Address.House, fixture.NewHouse);
            Assert.AreEqual(entity.Address.Appartment, fixture.NewAppartment);
            Assert.AreNotEqual(oldAddress.AddressId, entity.AddressId);
        }
    }
}
