using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.Login
{
    public class LoginQuery : IQuery<LoginResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
