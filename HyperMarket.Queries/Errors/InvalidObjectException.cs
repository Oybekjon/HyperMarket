using HyperMarket.Queries.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HyperMarket.Queries.Errors
{
    [Serializable]
    public class InvalidObjectException : ValidationException
    {
        public InvalidObjectException() { }
        public InvalidObjectException(string message) : base(message) { }
        public InvalidObjectException(string message, Exception ex) : base(message, ex) { }
        public InvalidObjectException(string message, ICollection<FieldError> errors) : base(message, errors)
        {
        }
        protected InvalidObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
