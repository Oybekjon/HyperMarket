using System;
using System.Runtime.Serialization;

namespace HyperMarket.Errors {
    [Serializable]
    public class AuthException : BaseException {
        public AuthException() { }
        public AuthException(String message) : base(message) { }
        public AuthException(String message, Exception inner) : base(message, inner) { }
        protected AuthException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}