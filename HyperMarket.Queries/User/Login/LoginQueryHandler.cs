using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.Login
{
    public class LoginQueryHandler : BusinessLogicQueryHandler<LoginQuery, LoginResult>
    {
        public override Task<LoginResult> Handle(LoginQuery input)
        {
            return Task.FromResult(new LoginResult());
        }
    }
}
