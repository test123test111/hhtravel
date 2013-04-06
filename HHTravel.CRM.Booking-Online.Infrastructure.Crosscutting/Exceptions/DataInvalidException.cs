using System;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Exceptions
{
    public class DataInvalidException : Exception, ISerializable
    {
        public DataInvalidException()
            : base()
        {
        }

        public DataInvalidException(string message)
            : base(message)
        {
        }

        public DataInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected DataInvalidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}