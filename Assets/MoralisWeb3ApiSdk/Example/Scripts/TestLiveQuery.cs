
using MoralisWeb3ApiSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
#if UNITY_WEBGL
using Cysharp.Threading.Tasks;
using Moralis.WebGL;
using Moralis.WebGL.Platform.Objects;
using Moralis.WebGL.Platform.Queries;
using Moralis.WebGL.Platform.Queries.Live;
#else
using System.Threading.Tasks;
using Moralis;
using Moralis.Platform.Objects;
using Moralis.Platform.Queries;
using Moralis.Platform.Queries.Live;
using Assets.Scripts;
#endif

public class PlayerData : MoralisObject
    {
        public long TokenCount { get; set; }
        public string Name { get; set; }
        public PlayerData() : base("PlayerData") { }
    }

public class TestLiveQuery
{
#if UNITY_WEBGL
    public static async UniTask DoLiveQuery()
    {
        var moralisQueryPlayerData = await MoralisInterface.GetClient().Query<PlayerData>();

        // Setup subscription
        setupLiveQuerySubscription(moralisQueryPlayerData);

        Thread.Sleep(2000);
    }

    private static void setupLiveQuerySubscription(MoralisQuery<PlayerData> playerData)
    {
        Moralis.MoralisLiveQueryCallbacks<PlayerData> moralisLiveQueryCallbacks = new Moralis.MoralisLiveQueryCallbacks<PlayerData>();

        moralisLiveQueryCallbacks.OnConnectedEvent += (() => { Debug.Log("Connection Established."); });
        moralisLiveQueryCallbacks.OnSubscribedEvent += ((requestId) => { Debug.Log($"Subscription {requestId} created."); });
        moralisLiveQueryCallbacks.OnUnsubscribedEvent += ((requestId) => { Debug.Log($"Unsubscribed from {requestId}."); });
        moralisLiveQueryCallbacks.OnErrorEvent += ((ErrorMessage em) =>
        {
            Debug.Log($"***** ERROR: code: {em.code}, msg: {em.error}, requestId: {em.requestId}");
        });
        moralisLiveQueryCallbacks.OnCreateEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Created {item.Name}");
        });
        moralisLiveQueryCallbacks.OnUpdateEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Updated ");
        });
        moralisLiveQueryCallbacks.OnDeleteEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Deleted");
        });
        moralisLiveQueryCallbacks.OnGeneralMessageEvent += ((text) =>
        {
            Debug.Log($"***** Websocket message: {text}");
        });
    //subscription = 
        Moralis.MoralisLiveQueryController.AddSubscription<PlayerData>("PlayerData", playerData, moralisLiveQueryCallbacks);
    }
#else
    public static async Task DoLiveQuery()
    {
        var moralisQueryPlayerData = MoralisInterface.GetClient().Query<PlayerData>();

        // Setup subscription
        setupLiveQuerySubscription(moralisQueryPlayerData);

        Thread.Sleep(2000);
    }

    private static void setupLiveQuerySubscription(MoralisQuery<PlayerData> playerData)
    {
        MoralisLiveQueryCallbacks<PlayerData> moralisLiveQueryCallbacks = new MoralisLiveQueryCallbacks<PlayerData>();

        moralisLiveQueryCallbacks.OnConnectedEvent += (() => { Debug.Log("Connection Established."); });
        moralisLiveQueryCallbacks.OnSubscribedEvent += ((requestId) => { Debug.Log($"Subscription {requestId} created."); });
        moralisLiveQueryCallbacks.OnUnsubscribedEvent += ((requestId) => { Debug.Log($"Unsubscribed from {requestId}."); });
        moralisLiveQueryCallbacks.OnErrorEvent += ((ErrorMessage em) =>
        {
            Debug.Log($"***** ERROR: code: {em.code}, msg: {em.error}, requestId: {em.requestId}");
        });
        moralisLiveQueryCallbacks.OnCreateEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Created {item.Name}");
        });
        moralisLiveQueryCallbacks.OnUpdateEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Updated ");
        });
        moralisLiveQueryCallbacks.OnDeleteEvent += ((item, requestId) =>
        {
            Debug.Log($"***** Deleted");
        });
        moralisLiveQueryCallbacks.OnGeneralMessageEvent += ((text) =>
        {
            Debug.Log($"***** Websocket message: {text}");
        });
        //subscription = 
        MoralisLiveQueryController.AddSubscription<PlayerData>("PlayerData", playerData, moralisLiveQueryCallbacks);
    }
#endif
}

