using System.Runtime.Serialization;

namespace FreeCourse.Web.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException():base()
        {
        }

        public UnauthorizeException(string? message) : base(message)
        {
        }

        public UnauthorizeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnauthorizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
