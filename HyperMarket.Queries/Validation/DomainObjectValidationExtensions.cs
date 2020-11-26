using System;
using System.Collections.Generic;
using System.Text;
using HMUser = HyperMarket.DomainObjects.User;

namespace HyperMarket.Queries.Validation
{
    internal static class DomainObjectValidationExtensions
    {
        public static bool IsFullUser(this HMUser user)
        {
            if (user == null)
                throw new NullReferenceException();
            return
                (
                    !string.IsNullOrWhiteSpace(user.PhoneNumber) &&
                    user.PhoneNumberConfirmed
                ) ||
                (
                    !string.IsNullOrWhiteSpace(user.Email) &&
                    user.EmailConfirmed
                );
        }
    }
}
