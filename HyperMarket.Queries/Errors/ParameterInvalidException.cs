using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HyperMarket.Queries.Errors
{
    [Serializable]
    public class ParameterInvalidException : ValidationException
    {
        public ParameterInvalidException() { }
        public ParameterInvalidException(string message) : base(message) { }
        public ParameterInvalidException(string message, Exception ex) : base(message, ex) { }
        public ParameterInvalidException(string message, ICollection<FieldError> errors) : base(message, errors)
        {
        }
        protected ParameterInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
