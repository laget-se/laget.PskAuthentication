using System;

namespace laget.PskAuthentication.Core.Exceptions
{
    public class PskExpiredException : Exception
    {
        public PskExpiredException()
        {
        }

        public PskExpiredException(string message)
            : base(message)
        {
        }

        public PskExpiredException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}