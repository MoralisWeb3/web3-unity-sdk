using UnityEngine;
using System;
using MoralisUnity.Sdk.Events;

namespace MoralisUnity.Sdk.Data
{
    /// <summary>
    /// Wrapper so a type can be observable via events
    /// </summary>
    public class Observable<T> where T : struct, IConvertible
    {
        //  Events ----------------------------------------
        public ObservableUnityEvent<T> OnValueChanged = new ObservableUnityEvent<T>();
        
        //  Properties ------------------------------------
        public T Value
        {
            set
            {
                _value = OnValueChanging(_value, value);
                OnValueChanged.Invoke(_value);
            }
            get
            {
                return _value;
                
            }
        }

        //  Fields ----------------------------------------
        private T _value;
        
        //  Constructor Methods ---------------------------
        static Observable()
        {
            // Limitation is based on current use, not theoretical use.
            if (typeof(T).IsEnum == false)
            {
                throw new ArgumentException("Observable T must be of type enum");
            }
        }

        public Observable ()
        {
            
        }

        //  Methods ---------------------------------------
        protected virtual T OnValueChanging (T oldValue, T newValue)
        {
            return newValue;
        }
        
        //  Event Handlers --------------------------------
    }
}