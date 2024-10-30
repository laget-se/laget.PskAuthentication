using System;

namespace laget.PskAuthentication.Core
{
    public class PskMalformedException : Exception
    {
        public PskMalformedException()
        {
        }

        public PskMalformedException(string message)
            : base(message)
        {
        }

        public PskMalformedException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
