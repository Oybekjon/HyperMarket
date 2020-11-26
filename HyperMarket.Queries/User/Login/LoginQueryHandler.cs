using HyperMarket.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.Login
{
    public class LoginQueryHandler : BusinessLogicQueryHandler<LoginQuery, LoginResult>
    {
        private readonly RepositoryContextBase Repo;
        public LoginQueryHandler(RepositoryContextBase repo)
        {
            Repo = repo;
        }

        public override Task<LoginResult> Handle(LoginQuery input)
        {
            return Task.FromResult(new LoginResult());
        }
    }
}
