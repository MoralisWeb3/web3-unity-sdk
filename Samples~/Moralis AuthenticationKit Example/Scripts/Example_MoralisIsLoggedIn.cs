using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoralisUnity.Examples.AuthenticationKitDemo
{
    /// <summary>
    /// Checks Moralis.IsLoggedIn() in standalone scene
    /// </summary>
    public class Example_MoralisIsLoggedIn : MonoBehaviour
    {
        protected async void Start()
        {
            //TODO HACK: Remove this delay. Needed due to SDK changes in SDK v1.2.0
            await UniTask.Delay(500);

            Debug.Log("IsLoggedIn: " + Moralis.IsLoggedIn());
        }
    }
}
