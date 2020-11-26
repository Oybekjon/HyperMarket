using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.UserManagement.GetPermissions
{
    [TestClass]
    public class GetPermissionQueryHandlerTests
    {
        [TestMethod]
        public async Task GetPermissions_Ok()
        {
            using var fixture = new GetPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            result.Permissions.AssertContains(x => x == fixture.AddedPermission);
        }

        [TestMethod]
        public async Task GetPermissions_IncorrectUserId_Fail()
        {
            using var fixture = new GetPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(userId: 0);

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task GetPermissions_UserNotFound_Fail()
        {
            using var fixture = new GetPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(userId: fixture.NonExistingUserId);

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }
    }
}
