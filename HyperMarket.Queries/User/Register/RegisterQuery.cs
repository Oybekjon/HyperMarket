using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.Register
{
    public class RegisterQuery : IQuery<RegisterResult>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
