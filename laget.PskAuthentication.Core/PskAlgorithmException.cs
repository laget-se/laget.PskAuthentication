using System;

namespace laget.PskAuthentication.Core
{
    public class PskAlgorithmException : Exception
    {
        public PskAlgorithmException()
        {
        }

        public PskAlgorithmException(string message)
            : base(message)
        {
        }

        public PskAlgorithmException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
