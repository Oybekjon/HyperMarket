using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Linq;

namespace HyperMarket.Queries.Store.AssignUser.Validation
{
    public class AssignUserQueryValidator : BaseValidator<AssignUserQuery>
    {
        public AssignUserQueryValidator()
        {
            RuleFor(x => x.StoreId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleForEach(x => x.Permissions)
                .Must(x => HyperMarketClaims.StoreClaims().Contains(x));
        }
    }
}
