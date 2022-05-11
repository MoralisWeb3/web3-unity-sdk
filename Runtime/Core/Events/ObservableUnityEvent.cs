using UnityEngine.Events;
using MoralisUnity.Sdk.Data;
using System;

namespace MoralisUnity.Sdk.Events
{
    /// <summary>
    /// The main event for <see cref="Observable{t}"/>.
    /// </summary>
    public class ObservableUnityEvent<T> : UnityEvent<T> where T : struct, IConvertible
    {
    }
}