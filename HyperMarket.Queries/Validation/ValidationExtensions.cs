using FluentValidation;
using HyperMarket.Errors;
using HyperMarket.Queries.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperMarket.Queries.Validation
{
    public static class ValidationExtensions
    {
        private static bool Validate<T>(AbstractValidator<T> validator, T data, ref FieldError[] errors)
        {
            var validationResult = validator.Validate(data);
            if (!validationResult.IsValid)
            {
                errors = validationResult.Errors.Select(x => new FieldError(x.PropertyName, x.ErrorCode, x.ErrorMessage))
                    .ToArray();
                return false;
            }
            return true;
        }

        public static void ValidateAndThrowInvalidObjectException<T>(this AbstractValidator<T> validator, T data, string exceptionMessage = "Parameter is not valid")
        {
            var errors = (FieldError[])null;
            if (!Validate(validator, data, ref errors))
            {
                throw new InvalidObjectException(exceptionMessage, errors);
            }
        }

        public static void ValidateObject<T>(this AbstractValidator<T> validator, T data, string exceptionMessage = "Parameter is not valid")
        {
            FieldError[] errors = null;
            if (!Validate(validator, data, ref errors))
            {
                var sb = new StringBuilder(exceptionMessage).AppendLine();
                foreach (var item in errors)
                {
                    sb.Append($"{item.Field}: {item.Message}").AppendLine();
                }
                throw new ParameterInvalidException(sb.ToString(), errors);
            }
        }
    }
}
