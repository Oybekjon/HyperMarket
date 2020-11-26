using FluentValidation;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.User.Register.Validation
{
    public class RegisterQueryValidator : BaseValidator<RegisterQuery>
    {
        public override string NullObjectExceptionMessage =>
            "Please provide registration model";

        public RegisterQueryValidator()
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty()
                .WithMessage("User identifier must be present");
            RuleFor(x => x.Email);
            RuleFor(x => x.Password)
                .NotEmpty()
                .When(x =>
                    !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Password)
                .Equal(x => x.PasswordConfirmation)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Passwords don't match");
        }
    }
}
