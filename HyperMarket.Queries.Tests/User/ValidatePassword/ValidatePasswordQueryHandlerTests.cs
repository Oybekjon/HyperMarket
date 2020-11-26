using HyperMarket.Errors;
using HyperMarket.Queries.User.ValidatePassword;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.Tests.User.ValidatePassword
{
    [TestClass]
    public class ValidatePasswordQueryHandlerTests
    {
        [TestMethod]
        public async Task ValidatePassword_Ok()
        {
            using var fixture = new ValidatePasswordQueryHandlerFixture();
            var query = fixture.CreateService();
            var password = "T3st_P@ssword1@!";

            var result = await query.Handle(new ValidatePasswordQuery
            {
                Password = password,
                PasswordConfirmation = password
            });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ValidatePassword_Empty_Fail()
        {
            using var fixture = new ValidatePasswordQueryHandlerFixture();
            var handler = fixture.CreateService();
            var password = "";

            await Assert.ThrowsExceptionAsync<WeakPasswordException>(
                () => handler.Handle(
                    new ValidatePasswordQuery
                    {
                        Password = password,
                        PasswordConfirmation = password
                    }
                )
            );
        }


        [TestMethod]
        public async Task ValidatePassword_WeakPopularPassword_Fail()
        {
            using var fixture = new ValidatePasswordQueryHandlerFixture();
            var handler = fixture.CreateService();
            var password = "123456";

            await Assert.ThrowsExceptionAsync<WeakPasswordException>(
                () => handler.Handle(
                    new ValidatePasswordQuery
                    {
                        Password = password,
                        PasswordConfirmation = password
                    }
                )
            );
        }

        [TestMethod]
        public async Task ValidatePassword_DigitOnly_Fail()
        {
            using var fixture = new ValidatePasswordQueryHandlerFixture();
            var handler = fixture.CreateService();
            var password = "123456789";

            await Assert.ThrowsExceptionAsync<WeakPasswordException>(
                () => handler.Handle(
                    new ValidatePasswordQuery
                    {
                        Password = password,
                        PasswordConfirmation = password
                    }
                )
            );
        }
    }
}
