using System;

namespace laget.PskAuthentication.Core.Exceptions
{
    public class UnsupportedAlgorithmException : Exception
    {
        public UnsupportedAlgorithmException()
        {
        }

        public UnsupportedAlgorithmException(string message)
            : base(message)
        {
        }

        public UnsupportedAlgorithmException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
