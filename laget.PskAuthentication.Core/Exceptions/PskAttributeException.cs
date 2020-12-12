﻿using System;

namespace laget.PskAuthentication.Core.Exceptions
{
    public class PskAttributeException : Exception
    {
        public PskAttributeException()
        {
        }

        public PskAttributeException(string message)
            : base(message)
        {
        }

        public PskAttributeException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
