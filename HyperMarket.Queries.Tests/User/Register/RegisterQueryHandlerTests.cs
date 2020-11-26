using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using HyperMarket.Queries.User.Register;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.User.Register
{
    [TestClass]
    public class RegisterQueryHandlerTests
    {
        [TestMethod]
        public async Task Register_Light_Ok()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();

            var result = await handler.Handle(new RegisterQuery
            {
                UserIdentifier = fixture.NewUserIdentifier
            });

            var entity = fixture.Context.Users.FirstOrDefault(
                x => x.UserIdentifier == fixture.NewUserIdentifier
            );
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public async Task Register_Full_Ok()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery();

            var result = await handler.Handle(query);

            var entity = fixture.Context.Users.FirstOrDefault(
                x => x.UserIdentifier == fixture.NewUserIdentifier
            );

            Assert.IsNotNull(entity);
            Assert.AreEqual(fixture.NewUserEmail, entity.Email);
            Assert.AreEqual(fixture.NewUserPhone, entity.PhoneNumber);
        }

        [TestMethod]
        public async Task Register_Full_MissingPassword_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery();
            query.Password = null;
            query.PasswordConfirmation = null;

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_MissingPasswordConfirmation_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery();
            query.PasswordConfirmation = null;

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_PasswordMismatch_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery(
                passwordConfirmation: fixture.NewUserPassword + "1"
            );

            await Assert.ThrowsExceptionAsync<ParameterInvalidException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_PasswordWeak_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery(
                passwordConfirmation: "1",
                password: "1"
            );

            await Assert.ThrowsExceptionAsync<WeakPasswordException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_PasswordWeak_2_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery(
                passwordConfirmation: "123456789",
                password: "123456789"
            );

            await Assert.ThrowsExceptionAsync<WeakPasswordException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_DuplicateEmail_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery(
                email: fixture.FullUserEmail
            );

            await Assert.ThrowsExceptionAsync<DuplicateEntryException>(() => handler.Handle(query));
        }

        [TestMethod]
        public async Task Register_Full_DuplicatePhone_Fail()
        {
            using var fixture = new RegisterQueryHandlerFixture();
            var handler = fixture.CreateService();
            var query = fixture.GetQuery(
                phoneNumber: fixture.FullUserPhone
            );

            await Assert.ThrowsExceptionAsync<DuplicateEntryException>(() => handler.Handle(query));
        }
    }
}
