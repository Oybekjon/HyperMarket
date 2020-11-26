using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.Store.Create
{
    [TestClass]
    public class CreateStoreQueryHandlerTests
    {
        [TestMethod]
        public async Task CreateStore_Ok()
        {
            using var fixture = new CreateStoreQueryHandlerFixture();
            var service = fixture.CreateService();
            var dto = fixture.GetQuery();

            var result = await service.Handle(dto);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StoreId > 0);
            Assert.IsNull(result.AddressId);
            Assert.AreEqual(fixture.NewStoreName, result.Name);
        }

        [TestMethod]
        public async Task CreateStore_ExistingAddress_Ok()
        {
            using var fixture = new CreateStoreQueryHandlerFixture();
            var service = fixture.CreateService();
            var dto = fixture.GetQuery(
                addressId: fixture.AddressId
            );

            var result = await service.Handle(dto);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StoreId > 0);
            Assert.AreEqual(fixture.AddressId, result.AddressId);
            Assert.AreEqual(fixture.NewStoreName, result.Name);
        }

        [TestMethod]
        public async Task CreateStore_NewAddress_Ok()
        {
            using var fixture = new CreateStoreQueryHandlerFixture();
            var service = fixture.CreateService();
            var dto = fixture.GetQuery(
                city: "Tashkent",
                streetAddress: "Some street",
                house: "19",
                appartment: "34"
            );

            var result = await service.Handle(dto);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StoreId > 0);
            Assert.IsNotNull(result.AddressId);
            Assert.IsTrue(result.AddressId.Value > 0);
            Assert.AreEqual(fixture.NewStoreName, result.Name);
        }
    }
}
