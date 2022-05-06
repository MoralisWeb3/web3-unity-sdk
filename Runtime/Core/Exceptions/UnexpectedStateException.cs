using UnityEngine;
using System;
using System.Collections.Generic;

namespace MoralisUnity.Sdk.Exceptions
{
    /// <summary>
    /// Creates elegant exception flow for unintended states.
    /// </summary>
    public class UnexpectedStateException : Exception
    {
        //  Properties ------------------------------------

        
        //  Fields ----------------------------------------

        
        //  Constructor Methods ---------------------------
        public UnexpectedStateException(object actual, object expected) :
            base($"Unexpected State Exception. actual = {actual}, expected = {expected.ToString()}")
        {
            
        }
        
        //  Methods ---------------------------------------

        
        //  Event Handlers --------------------------------
    }
}