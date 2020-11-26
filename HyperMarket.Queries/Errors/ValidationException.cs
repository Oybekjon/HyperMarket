using HyperMarket.Errors;
using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HyperMarket.Queries.Errors
{
   
    [Serializable]
    public class ValidationException : BaseException
    {
        private readonly Dictionary<string, FieldError> _errors = new Dictionary<string, FieldError>();

        public IReadOnlyDictionary<string, FieldError> Errors => _errors;

        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception ex) : base(message, ex) { }

        public ValidationException(string message, ICollection<FieldError> errors) : base(message)
        {
            if (errors != default)
            {
                foreach (var error in errors)
                {
                    _errors[error.Field] = error;
                }
            }
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
