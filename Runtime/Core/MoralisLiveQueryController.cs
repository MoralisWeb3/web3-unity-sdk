/**
 *           Module: MoralisLiveQueryController.cs
 *  Descriptiontion: A class that autonmates subscription handling for vaious 
 *                   game cycles.
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
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;
using MoralisUnity;

namespace MoralisUnity
{
    /// <summary>
    /// A class that autonmates subscription handling for vaious 
    /// game cycles.
    /// </summary>
    public class MoralisLiveQueryController : MonoBehaviour
    {
        // Singleton instance.
        private static MoralisLiveQueryService instance = new MoralisLiveQueryService();

        private void OnDestroy()
        {
            Debug.Log("MoralisLiveQueryController - OnDestroy called.");
            instance.UnsubscribeFromAll();
            instance.subscriptions.Clear();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("MoralisLiveQueryController - OnApplicationQuit called.");
            instance.UnsubscribeFromAll();
            instance.subscriptions.Clear();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log("MoralisLiveQueryController - OnApplicationPause called.");
            instance.UnsubscribeFromAll();
        }

        protected void Awake()
        {
            Debug.Log("MoralisLiveQueryController - Awake called.");
            List<UniTask> tasks = new List<UniTask>();

            foreach (string key in instance.subscriptions.Keys)
            {
                Debug.Log($"Resubscribing to {key}");
                tasks.Add(instance.subscriptions[key].RenewSubscription());
            }

            UniTask.WhenAll(tasks.ToArray());
        }


        /// <summary>
        /// Add a subscription for a query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName"></param>
        /// <param name="q"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ISubscriptionQuery AddSubscription<T>(string keyName, MoralisQuery<T> q, MoralisLiveQueryCallbacks<T> c) where T : MoralisObject
        {
            ISubscriptionQuery resp = null;

            if (!instance.subscriptions.ContainsKey(keyName))
            {
                resp = new MoralisSubscriptionQuery<T>(keyName, q, c);
            }
            else
            {
                Debug.LogError($"{keyName} already exists cannot create a duplicate.");
            }

            return resp;
        }

        /// <summary>
        /// Retrieves the specified subscription object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static MoralisSubscriptionQuery<T> GetSubscription<T>(string keyName) where T : MoralisObject
        {
            MoralisSubscriptionQuery<T> resp = null;

            if (instance.subscriptions.ContainsKey(keyName))
            {
                resp = (MoralisSubscriptionQuery<T>)instance.subscriptions[keyName];
            }

            return resp;
        }

        /// <summary>
        /// Removes specified subscription.
        /// </summary>
        /// <param name="keyName"></param>
        public static async void RemoveSubscriptions(string keyName)
        {
            if (instance.subscriptions.ContainsKey(keyName))
            {
                await instance.subscriptions[keyName].Unsubscribe();

                instance.subscriptions.Remove(keyName);
            }
        }
    }

    internal class MoralisLiveQueryService
    {
        public Dictionary<string, ISubscriptionQuery> subscriptions;

        public MoralisLiveQueryService()
        {
            subscriptions = new Dictionary<string, ISubscriptionQuery>();
        }

        public void UnsubscribeFromAll()
        {
            List<UniTask> tasks = new List<UniTask>();

            foreach (string key in subscriptions.Keys)
            {
                Debug.Log($"Unsubscribing from {key}");
                tasks.Add(subscriptions[key].Unsubscribe());
            }

            UniTask.WhenAll(tasks.ToArray());
        }
    }
}