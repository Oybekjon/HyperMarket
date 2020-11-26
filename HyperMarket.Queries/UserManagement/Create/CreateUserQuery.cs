using System;

namespace HyperMarket.Queries.UserManagement.Create
{
    public class CreateUserQuery : IQuery<CreateUserResult>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? StoreId { get; set; }
    }
}