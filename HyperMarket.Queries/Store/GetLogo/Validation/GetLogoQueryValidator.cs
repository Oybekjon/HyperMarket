using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Store.GetLogo.Validation
{
    public class GetLogoQueryValidator : BaseValidator<GetLogoQuery>
    {
        public GetLogoQueryValidator()
        {
            RuleFor(x=>x.StoreId).GreaterThan(0);
            RuleFor(x => x.Width).GreaterThan(0).When(x => x.Width.HasValue);
            RuleFor(x => x.Height).GreaterThan(0).When(x => x.Height.HasValue);
        }
    }
}
