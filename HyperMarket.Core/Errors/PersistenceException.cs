using System;
using System.Runtime.Serialization;

namespace HyperMarket.Errors {
    [Serializable]
    public class PersistenceException : BaseException {
        public PersistenceException() { }
        public PersistenceException(String message) : base(message) { }
        public PersistenceException(String message, Exception inner) : base(message, inner) { }
        protected PersistenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}