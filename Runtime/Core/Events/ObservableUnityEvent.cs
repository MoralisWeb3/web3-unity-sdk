using UnityEngine;
using UnityEngine.Events;
using MoralisUnity.Data;
using System;

namespace MoralisUnity.Events
{
    /// <summary>
    /// The main event for <see cref="Observable{t}"/>.
    /// 
    /// </summary>
    public class ObservableUnityEvent<T> : UnityEvent<T> where T : struct, IConvertible
    {
    }
}