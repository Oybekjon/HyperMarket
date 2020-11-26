using HyperMarket.Queries.UserManagement.GetList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.UserManagement.GetList
{
    [TestClass]
    public class GetUserListQueryHandlerTests
    {
        [TestMethod]
        public async Task GetUsers_All_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery());

            all.Data.AssertContains(x => x.UserId == fixture.ExistingUserId);
            all.Data.AssertContains(x => x.UserId == fixture.FullUserId);
            all.Data.AssertContains(x => x.UserId == fixture.U1UserId);
            all.Data.AssertContains(x => x.UserId == fixture.U2UserId);
            all.Data.AssertContains(x => x.UserId == fixture.U3UserId);
        }

        [TestMethod]
        public async Task GetUsers_OnlyFull_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                FullOnly = true
            });

            all.Data.AssertNotContains(x => x.UserId == fixture.ExistingUserId);
            all.Data.AssertContains(x => x.UserId == fixture.FullUserId);
            all.Data.AssertContains(x => x.UserId == fixture.U1UserId);
            all.Data.AssertContains(x => x.UserId == fixture.U2UserId);
            all.Data.AssertContains(x => x.UserId == fixture.U3UserId);
        }

        [TestMethod]
        public async Task GetUsers_NameQuery_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                NameQuery = "Nic"
            });

            all.Data.AssertNotContains(x => x.UserId == fixture.ExistingUserId);
            all.Data.AssertNotContains(x => x.UserId == fixture.FullUserId);
            all.Data.AssertContains(x => x.UserId == fixture.U1UserId);
            all.Data.AssertNotContains(x => x.UserId == fixture.U2UserId);
            all.Data.AssertContains(x => x.UserId == fixture.U3UserId);
        }

        [TestMethod]
        public async Task GetUsers_ByEmail_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                Email = fixture.U1Email
            });

            all.Data.AssertContains(x => x.Email == fixture.U1Email);
        }

        [TestMethod]
        public async Task GetUsers_ByPhone_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                PhoneNumber = fixture.U1PhoneNumber
            });

            all.Data.AssertContains(x => x.PhoneNumber == fixture.U1PhoneNumber);
        }

        [TestMethod]
        public async Task GetUsers_ByStoreId_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                StoreId = fixture.StoreId
            });

            all.Data.AssertContains(x => x.UserId == fixture.FullUserId);
        }

        [TestMethod]
        public async Task GetUsers_ByUserId_Ok()
        {
            using var fixture = new GetUserListQueryHandlerFixture();
            var service = fixture.CreateService();

            var all = await service.Handle(new GetUserListQuery
            {
                UserId = fixture.U2UserId
            });

            all.Data.AssertContains(x => x.UserId == fixture.U2UserId);
        }
    }
}
