using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using HyperMarket.Queries.Store.AssignUserPermissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.Store.AssignUserPermissions
{
    [TestClass]
    public class AssignUserPermissionQueryHandlerTests
    {
        [TestMethod]
        public async Task AssignPermission_Ok()
        {
            using var fixture = new AssignUserPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var permission = fixture.NotAddedClaim;
            var query = fixture.GetQuery<AssignUserPermissionQuery>(
                permissions: new List<string> { permission }
            );

            await service.Handle(query);

            var entity = fixture.Context.UserStorePermissions.FirstOrDefault(x =>
                x.StoreId == fixture.StoreId &&
                x.UserId == fixture.FullUserId &&
                x.Permission == permission
            );

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public async Task AssignPermission_EmptyPermissions_Fail()
        {
            using var fixture = new AssignUserPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery<AssignUserPermissionQuery>(
                permissions: new List<string>()
            );

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignPermission_NonExistentCombination_Fail()
        {
            using var fixture = new AssignUserPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var permission = fixture.NotAddedClaim;
            var query = fixture.GetQuery<AssignUserPermissionQuery>(
                permissions: new List<string> { permission },
                userId: fixture.ExistingUserId
            );

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }
    }
}
