using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HyperMarket.Errors {
    [Serializable]
    public class DatabaseException : BaseException {
        public DatabaseException() { }
        public DatabaseException(String message) : base(message) { }
        public DatabaseException(String message, Exception inner) : base(message, inner) { }
        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
