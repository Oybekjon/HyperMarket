using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using HyperMarket.Queries.Store.AssignUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.Store.AssignUser
{
    [TestClass]
    public class AssignUserQueryHandlerTests
    {
        [TestMethod]
        public async Task AssignUser_Ok()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.UnassignedUserId
            );

            await service.Handle(query);

            var entity = fixture.Context.UserStores.FirstOrDefault(
                x => x.UserId == fixture.UnassignedUserId &&
                     x.StoreId == fixture.StoreId
            );
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public async Task AssignUser_Permissions_Ok()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.UnassignedUserId,
                permissions: new List<string> {
                    HyperMarketClaims.StoreDetailsManager
                }
            );

            await service.Handle(query);

            var entity = fixture.Context.UserStores.FirstOrDefault(
                x => x.UserId == fixture.UnassignedUserId &&
                     x.StoreId == fixture.StoreId
            );
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public async Task AssignUser_WrongPermissions_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.UnassignedUserId,
                permissions: new List<string> {
                    HyperMarketClaims.Admin,
                    HyperMarketClaims.StoreDetailsManager
                }
            );

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_NotAllowedStoreId_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.UnassignedUserId,
                storeId: 0
            );

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_NotAllowedUserId_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: 0
            );

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_NotExistingUser_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.NonExistingUserId
            );

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_NotExistingStore_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                storeId: fixture.NonExistingStoreId,
                userId: fixture.UnassignedUserId
            );

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_AssociationAlreadyExists_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.FullUserId
            );

            await Assert.ThrowsExceptionAsync<DuplicateEntryException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignUser_NotAllowedUser_Fail()
        {
            using var fixture = new AssignUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserQuery>(
                userId: fixture.ExistingUserId
            );

            await Assert.ThrowsExceptionAsync<NotAllowedException>(() => service.Handle(query));
        }
    }
}
