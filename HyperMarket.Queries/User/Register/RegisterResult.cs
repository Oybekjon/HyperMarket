using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.Register
{
    public class RegisterResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long UserId { get; set; }
    }
}
