using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Validation
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public virtual string NullObjectExceptionMessage
        {
            get
            {
                return "Object equals null";
            }
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return context.InstanceToValidate == null
                ? new ValidationResult(new[] { new ValidationFailure(typeof(T).Name, NullObjectExceptionMessage) })
            : base.Validate(context);
        }
    }
}
