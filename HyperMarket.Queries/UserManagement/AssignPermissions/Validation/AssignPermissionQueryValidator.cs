using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Linq;

namespace HyperMarket.Queries.UserManagement.AssignPermissions.Validation
{
    public class AssignPermissionQueryValidator : BaseValidator<AssignPermissionQuery>
    {
        public AssignPermissionQueryValidator()
        {
            RuleForEach(x => x.Permissions)
                .Must(x => HyperMarketClaims.NonStoreClaims().Contains(x));

            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
