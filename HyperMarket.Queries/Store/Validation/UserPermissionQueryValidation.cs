using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperMarket.Queries.Store.Validation
{
    public class UserPermissionQueryValidation : BaseValidator<UserPermissionQuery>
    {
        public UserPermissionQueryValidation()
        {
            RuleFor(x => x.Permissions)
                .NotEmpty();

            RuleFor(x => x.StoreId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleForEach(x => x.Permissions)
                .Must(x => HyperMarketClaims.StoreClaims().Contains(x));
        }
    }
}
