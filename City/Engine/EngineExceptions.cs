using System;
using System.Runtime.Serialization;

namespace City.Engine.Exceptions
{
    public class NullOnwerException : Exception
    {
        public NullOnwerException()
        {
        }

        public NullOnwerException(string message) : base(message)
        {
        }

        public NullOnwerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NullOnwerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }



    [Serializable]
    public class LoadFailException : Exception
    {
        public LoadFailException()
        {
        }

        public LoadFailException(string message) : base(message)
        {
        }

        public LoadFailException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoadFailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
