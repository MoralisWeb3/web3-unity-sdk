﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoralisUnity
{
    /// <summary>
    /// Provides a persistable object to hold Moralis related data.
    /// </summary>
    [Serializable]
    public class MoralisServerSettings : ScriptableObject
    {
        [FormerlySerializedAs("ServerUri")] public string ServerUrl;
        public string ApplicationId;
        public string ApplicationName;
        public string ApplicationDescription;
        public string ApplicationVersion;
        public string ApplicationIconUri;
        public string ApplicationUrl;
        public bool DisableAutoOpenWizard = false;

        public MoralisServerSettings()
        {
            ServerUrl = String.Empty;
            ApplicationId = String.Empty;
            ApplicationName = "Moralis SDK Application";
            ApplicationDescription = "This application provides an example of how to you Moralis in a Unity 3D Game";
            ApplicationVersion = "1.0.0";
            ApplicationUrl = "https://moralis.io";
            ApplicationIconUri = "https://moralis.io/wp-content/uploads/2021/06/Powered-by-Moralis-Badge-Black.svg";
        }
    }
}