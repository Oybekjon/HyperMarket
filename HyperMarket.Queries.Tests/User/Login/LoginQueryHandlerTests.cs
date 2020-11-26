using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMarket.DomainObjects;
using HyperMarket.Queries.Tests.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HyperMarket.Queries.Tests.User.Login
{
    [TestClass]
    public class LoginQueryHandlerTests
    {
        [TestMethod]
        public async Task Login_Ok()
        {
            using var fixture = new LoginQueryHandlerFixture();
            var handler = fixture.CreateService();

            var result = await handler.Handle(new Queries.User.Login.LoginQuery { });

            Assert.IsNotNull(result);
        }
    }
}
