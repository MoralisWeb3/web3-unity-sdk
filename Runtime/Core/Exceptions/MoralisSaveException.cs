using System;

namespace MoralisUnity.Core.Exceptions
{
    public class MoralisSaveException : Exception
    {
        public MoralisSaveException() { }

        public MoralisSaveException(string message) : base(message) { }
    }
}