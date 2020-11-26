using HyperMarket.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HyperMarket.Queries.User.ValidatePassword
{
    public class ValidatePasswordQueryHandler : BusinessLogicQueryHandler<ValidatePasswordQuery, ValidatePasswordResult>
    {
        public override Task<ValidatePasswordResult> Handle(ValidatePasswordQuery input)
        {
            var weekPasswordList = new[] {
                "123456",
                "password",
                "12345678",
                "qwerty",
                "abc123",
                "123456789",
                "111111",
                "1234567",
                "iloveyou",
                "adobe123",
                "123123",
                "admin",
                "1234567890",
                "letmein",
                "photoshop",
                "1234",
                "monkey",
                "shadow",
                "sunshine",
                "12345",
                "password1",
                "princess",
                "azerty",
                "trustno1",
                "000000"
            };
            if (input.Password != input.PasswordConfirmation)
            {
                throw ErrorHelper.InvalidArgument("Password and confirmations does not match");
            }
            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 6)
                throw new WeakPasswordException("Must be 6 characters long");
            if (weekPasswordList.Any(x => x == input.Password))
                throw new WeakPasswordException("Password is too weak");
            var chars = new Regex("[a-z]", RegexOptions.IgnoreCase);
            var digits = new Regex("\\d");
            if (!chars.IsMatch(input.Password))
                throw new WeakPasswordException("Must have at least one letter");
            if (!digits.IsMatch(input.Password))
                throw new WeakPasswordException("Must have at least one digit");
            return Task.FromResult(new ValidatePasswordResult());
        }
    }
}
