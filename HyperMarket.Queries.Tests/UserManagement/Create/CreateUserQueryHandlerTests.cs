using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.UserManagement.Create
{
    [TestClass]
    public class CreateUserQueryHandlerTests
    {
        [TestMethod]
        public async Task CreateUser_Ok()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();
            query.StoreId = null;

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var user = fixture.Context.Users.FirstOrDefault(x => x.UserIdentifier == fixture.NewUserIdentifier);
            Assert.IsNotNull(user);
            Assert.AreEqual(fixture.NewUserFirstName, user.FirstName);
            Assert.AreEqual(fixture.NewUserPhone, user.PhoneNumber);
        }

        [TestMethod]
        public async Task CreateUser_WithStore_Ok()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();

            var result = await service.Handle(query);

            Assert.IsNotNull(result);
            var user = fixture.Context.Users
                .Include(x => x.UserStores)
                .FirstOrDefault(x => x.UserIdentifier == fixture.NewUserIdentifier);
            Assert.IsNotNull(user);
            Assert.AreEqual(fixture.NewUserFirstName, user.FirstName);
            Assert.AreEqual(fixture.NewUserPhone, user.PhoneNumber);
            var storeAssoc = user.UserStores.FirstOrDefault(x => x.StoreId == fixture.StoreId);
            Assert.IsNotNull(storeAssoc);
        }


        [TestMethod]
        public async Task CreateUser_EmailOrPhoneNotProvided_Fail()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery();
            query.PhoneNumber = null;
            query.Email = null;

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task CreateUser_InvalidPhone_Fail()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(phoneNumber: "asdf");

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task CreateUser_InvalidEmail_Fail()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(email: "someEmail@");

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => service.Handle(query));
        }

        [TestMethod]
        public async Task CreateUser_NonExistingStore_Fail()
        {
            using var fixture = new CreateUserQueryHandlerFixture();
            var service = fixture.CreateService();
            var query = fixture.GetQuery(
                storeId: uint.MaxValue
            );

            await Assert.ThrowsExceptionAsync<NotFoundException>(() => service.Handle(query));
        }
    }
}
