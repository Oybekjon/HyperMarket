using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.UserManagement.AssignPermissions
{
    [TestClass]
    public class AssignPermissionQueryHandlerTests
    {
        [TestMethod]
        public async Task AssignPermission_Ok()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var entities = fixture.Context.UserPermissions
                .Where(x => x.UserId == query.UserId)
                .ToList();

            entities.AssertContains(x => x.Permission == fixture.NotAddedPermission);
            entities.AssertNotContains(x => x.Permission == fixture.AddedPermission);
        }

        [TestMethod]
        public async Task AssignPermission_Preserve_Ok()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();
            query.Permissions.Add(fixture.AddedPermission);

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var entities = fixture.Context.UserPermissions
                .Where(x => x.UserId == query.UserId)
                .ToList();

            entities.AssertContains(x => x.Permission == fixture.NotAddedPermission);
            entities.AssertContains(x => x.Permission == fixture.AddedPermission);
        }

        [TestMethod]
        public async Task AssignPermission_NonExistingUser_Fail()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(userId: fixture.NonExistingUserId);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignPermission_InvalidPermission_Fail()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(permission: fixture.InvalidPermission);

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignPermission_InvalidUserId_Fail()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(userId: 0);

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignPermission_IncompleteUser_Fail()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(userId: fixture.ExistingUserId);

            await Assert.ThrowsExceptionAsync<NotAllowedException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task AssignPermission_ClearPermissions_Ok()
        {
            using var fixture = new AssignPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();
            query.Permissions.Clear();

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var entities = fixture.Context.UserPermissions
                .Where(x => x.UserId == query.UserId)
                .ToList();

            entities.AssertNotContains(x => x.Permission == fixture.NotAddedPermission);
            entities.AssertNotContains(x => x.Permission == fixture.AddedPermission);
        }
    }
}
