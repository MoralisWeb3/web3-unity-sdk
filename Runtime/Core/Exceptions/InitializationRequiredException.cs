using System;
using MoralisUnity.Sdk.Interfaces;

namespace MoralisUnity.Sdk.Exceptions
{
    /// <summary>
    /// Exception thrown within <see cref="IInitializable"/> when
    /// improperly used.
    /// </summary>
    public class InitializationRequiredException : Exception
    {
        //  Properties  ------------------------------------

        //  General Methods  ------------------------------
        public InitializationRequiredException(IInitializable obj) : base()
        {
            
        }
        public InitializationRequiredException(IInitializableAsync obj) : base()
        {
            
        }
    }
}