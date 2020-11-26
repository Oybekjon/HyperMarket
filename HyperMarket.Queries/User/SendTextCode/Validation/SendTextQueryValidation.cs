using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HyperMarket.Queries.User.SendTextCode.Validation
{
    public class SendTextQueryValidation : BaseValidator<SendTextQuery>
    {
        public override string NullObjectExceptionMessage =>
            "Please provide text query";

        public SendTextQueryValidation()
        {
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Please provide Phone number")
                .Matches(@"^\d+$")
                .WithMessage("Invalid phone number");
        }
    }
}
