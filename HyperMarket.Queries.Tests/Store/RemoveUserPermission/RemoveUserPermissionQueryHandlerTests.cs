using HyperMarket.Queries.Store.RemoveUserPermission;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HyperMarket.Queries.Tests.Store.RemoveUserPermission
{
    [TestClass]
    public class RemoveUserPermissionQueryHandlerTests
    {
        [TestMethod]
        public async Task RemovePermission_Ok()
        {
            using var fixture = new RemoveUserPermissionQueryHandlerFixture();
            var service = fixture.CreateService();
            var addedClaim = fixture.AddedClaims.First();
            var query = fixture.GetQuery<RemoveUserPermissionQuery>(
                permissions: new List<string> { addedClaim }
            );

            var result = await service.Handle(query);
            Assert.IsNotNull(result);
            var entity = fixture.Context.UserStorePermissions.FirstOrDefault(
                x => 
                    x.StoreId == fixture.StoreId &&
                    x.UserId == fixture.FullUserId &&
                    x.Permission == addedClaim
            );
            Assert.IsNull(entity);
        }
    }
}
