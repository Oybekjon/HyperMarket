using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ViewModels.UserManagement
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailValidationCode { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsActive { get; set; }
    }
}
