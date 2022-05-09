using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoralisUnity.Examples.AuthenticationKitDemo
{
    /// <summary>
    /// Checks for logged in user in a standalone scene
    /// </summary>
    public class Example_MoralisIsLoggedIn : MonoBehaviour
    {
        protected async void Start()
        {
            Moralis.Start();
            
            var user = await Moralis.GetUserAsync();

            if (user != null)
            {
                Debug.Log("User logged in");
            }
        }
    }
}
