/**
 *           Module: MoralisSetup.cs
 *  Descriptiontion: Example class that demonstrates a game menu that incorporates
 *                   Wallet Connect and Moralis Authentication.
 *           Author: Moralis Web3 Technology AB, 559307-5988 - David B. Goodrich 
 *  
 *  MIT License
 *  
 *  Copyright (c) 2021 Moralis Web3 Technology AB, 559307-5988
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using System;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Services.ClientServices;
using UnityEngine;
using WalletConnectSharp.Unity;
using WalletConnectSharp.Core.Models;

namespace MoralisUnity
{
    /// <summary>
    /// Internal MonoBehaviour that allows Moralis to run an Update loop.
    /// </summary>
    public class MoralisSingleton : MonoBehaviour
    {
        /// <summary>
        /// Moralis client
        /// </summary>
        public MoralisClient Client { get; set; }
        
        /// <summary>Indicates that the app is closing. Set in OnApplicationQuit().</summary>
        [NonSerialized]
        public static bool AppQuits;
        
        private static MoralisSingleton instance;
        internal static MoralisSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MoralisSingleton>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "MoralisSingleton";
                        instance = obj.AddComponent<MoralisSingleton>();
                    }
                }

                return instance;
            }
        }
        
        /// <summary>Called by Unity when the application gets closed. The UnityEngine will also call OnDisable, which disconnects.</summary>
        protected void OnApplicationQuit()
        {
            AppQuits = true;
        }
        
        void Awake()
        {
            if (instance == null || ReferenceEquals(this, instance))
            {
                instance = this;
                DontDestroyOnLoad ( gameObject );
            }
            else
            {
                Destroy(this);
            }
        }

        void OnEnable()
        {
            if (Instance != this)
            {
                Debug.LogError("MoralisController is a singleton but there are multiple instances. this != Instance.");
                return;
            }
            
            this.Client = Moralis.Client;
        }
        
#if UNITY_WEBGL
        private void FixedUpdate()
        {
            MoralisLiveQueryManager.UpdateWebSockets();
        }
#endif
    }
}
