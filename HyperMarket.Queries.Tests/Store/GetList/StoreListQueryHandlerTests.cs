using HyperMarket.Queries.Store.GetList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.Store.GetList
{
    [TestClass]
    public class StoreListQueryHandlerTests
    {
        [TestMethod]
        public async Task GetList_Ok()
        {
            using var fixture = new StoreListQueryHandlerFixture();
            var service = fixture.CreateService();

            var result = await service.Handle(new StoreListQuery());

            Assert.IsNotNull(result);

            var containsBoth = result.Data.Count(x =>
                x.StoreId == fixture.StoreId ||
                x.StoreId == fixture.SecondStoreId
            ) == 2;

            Assert.IsTrue(containsBoth);
        }

        [TestMethod]
        public async Task GetList_ById_Ok()
        {
            using var fixture = new StoreListQueryHandlerFixture();
            var service = fixture.CreateService();

            var result = await service.Handle(new StoreListQuery { 
                StoreId = fixture.StoreId
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(result.Data[0].StoreId, fixture.StoreId);
        }
    }
}
