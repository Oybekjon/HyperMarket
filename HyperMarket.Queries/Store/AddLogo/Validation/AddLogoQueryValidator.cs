using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperMarket.Queries.Store.AddLogo.Validation
{
    public class AddLogoQueryValidator : BaseValidator<AddLogoQuery>
    {
        public AddLogoQueryValidator()
        {
            RuleFor(x => x.StoreId).GreaterThan(0);
            RuleFor(x => x.Image).NotNull();
            RuleFor(x => x.Image).Must(x => x.CanSeek).When(x => x != null);
            RuleFor(x => x.Image).Must(x => {
                var format = x.GetImageFormat();
                return new [] { 
                    ImageFormatType.Jpeg, 
                    ImageFormatType.Png, 
                    ImageFormatType.Gif 
                }.Contains(format);
            }).WithMessage("Only jpeg, png and gif images are allowed");
        }
    }
}
