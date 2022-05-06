using System;
using MoralisUnity.Sdk.Interfaces;

namespace MoralisUnity.Sdk.Exceptions
{
    /// <summary>
    /// Exception thrown within <see cref="IInitializable"/> when
    /// improperly used.
    /// </summary>
    public class InitializedAlreadyException : Exception
    {
        //  Properties  ------------------------------------

        //  General Methods  ------------------------------
        public InitializedAlreadyException (IInitializable obj) : base()
        {
        }
        public InitializedAlreadyException (IInitializableAsync obj) : base()
        {
        }
    }
}