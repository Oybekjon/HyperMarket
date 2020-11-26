using System.Collections.Generic;

namespace HyperMarket.Queries.User.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserIdentifier { get; set; }
        public IReadOnlyDictionary<long, string> StorePermissions { get; set; }
        public IReadOnlyCollection<string> Permissions { get; set; }
    }
}