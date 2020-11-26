using FluentValidation;
using HyperMarket.Queries.Validation;

namespace HyperMarket.Queries.UserManagement.GetPermissions.Validation
{
    public class GetPermissionQueryValidator : BaseValidator<GetPermissionQuery>
    {
        public GetPermissionQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
