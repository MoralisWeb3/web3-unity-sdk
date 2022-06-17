using System;

namespace MoralisUnity.Core.Exceptions
{
    public class MoralisSignupException : Exception
    {
        public MoralisSignupException() { }

        public MoralisSignupException(string message) : base(message) { }
    }
}