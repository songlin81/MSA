using System;
using System.Runtime.Serialization;
using System.Security;

namespace CustomerApi.Commons
{
    public class ServiceException : Exception
    {
        public ServiceException() : base() { }

        public ServiceException(string message) : base(message) { }

        [SecuritySafeCritical]
        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
