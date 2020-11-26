using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.ProductCategory.Validation
{
    public class ProductCategoryQueryValidator : BaseValidator<ProductCategoryQuery>
    {
        public ProductCategoryQueryValidator(bool canHaveEmptyId)
        {
            RuleFor(x => x.ProductCategory).NotEmpty();
            When(x => x.ProductCategory != null, () =>
            {
                RuleFor(x => x.ProductCategory.Name).NotEmpty();
                RuleFor(x => x.ProductCategory.NameUz).NotEmpty();
                RuleFor(x => x.ProductCategory.NameRu).NotEmpty();
                RuleFor(x => x.ProductCategory.ParentProductCategoryId)
                    .NotEqual(0)
                    .When(x => x.ProductCategory.ParentProductCategoryId.HasValue);

                if (!canHaveEmptyId)
                {
                    RuleFor(x => x.ProductCategory.ProductCategoryId)
                        .NotEmpty()
                        .GreaterThan(0);
                }
            });
        }
    }
}
