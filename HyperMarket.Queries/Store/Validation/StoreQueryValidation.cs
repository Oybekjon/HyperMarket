using FluentValidation;
using HyperMarket.Queries.Validation;

namespace HyperMarket.Queries.Store.Validation
{
    public class StoreQueryValidation : BaseValidator<StoreQuery>
    {
        public override string NullObjectExceptionMessage =>
            "Please provide store details";

        public StoreQueryValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be provided");
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name must be present");
        }
    }
}
