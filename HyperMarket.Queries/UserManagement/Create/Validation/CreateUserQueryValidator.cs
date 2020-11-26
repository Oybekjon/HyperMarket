using FluentValidation;
using HyperMarket.Queries.Validation;
using HyperMarket.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using HyperMarket;
using System.Text.RegularExpressions;

namespace HyperMarket.Queries.UserManagement.Create.Validation
{
    public class CreateUserQueryValidator : BaseValidator<CreateUserQuery>
    {
        private static readonly Regex DigitOnlyPattern = new Regex("^\\d{5,}$");
        public override string NullObjectExceptionMessage =>
            "Please provide the query";

        public CreateUserQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("You must provide email or phone");

            RuleFor(x => x.Email)
                .Matches(FormatValidator.EmailPattern)
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.Email))
                .WithMessage("You must provide email or phone");

            RuleFor(x => x.PhoneNumber)
                .Must(x => DigitOnlyPattern.IsMatch(x.NormalizePhone()))
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.UserIdentifier)
                .NotEmpty()
                .WithMessage("Must provide user identifier");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is compulsory");
        }
    }
}
