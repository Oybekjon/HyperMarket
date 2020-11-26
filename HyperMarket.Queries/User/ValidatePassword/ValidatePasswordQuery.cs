using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.ValidatePassword
{
    public class ValidatePasswordQuery : IQuery<ValidatePasswordResult>
    {
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
