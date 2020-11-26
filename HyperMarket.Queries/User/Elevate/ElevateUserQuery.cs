using System;

namespace HyperMarket.Queries.User.Elevate
{
    public class ElevateUserQuery : IQuery<ElevateUserResult>
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}