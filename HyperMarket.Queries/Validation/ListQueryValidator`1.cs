using HyperMarket.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace HyperMarket.Queries.Validation
{
    public class ListQueryValidator<T, TResult> : BaseValidator<T> 
        where T : ListQueryViewModel<TResult>
    {
        public ListQueryValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Offset.HasValue);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .When(x => x.Limit.HasValue);
        }
    }
}
