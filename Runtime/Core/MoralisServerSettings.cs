using System;
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
        [FormerlySerializedAs("ServerUrl")]
        [FormerlySerializedAs("ServerUri")] 
        public string DappUrl;
        [FormerlySerializedAs("ApplicationId")] public string DappId;
        [FormerlySerializedAs("ApplicationName")] public string DappName;
        [FormerlySerializedAs("ApplicationDescription")] public string DappDescription;
        [FormerlySerializedAs("ApplicationVersion")] public string DappVersion;
        [FormerlySerializedAs("DappIconUri")] [FormerlySerializedAs("ApplicationIconUri")] public string DappIconUrl;
        [FormerlySerializedAs("DappWebsite")] [FormerlySerializedAs("ApplicationUrl")] public string DappWebsiteUrl;
        public bool DisableAutoOpenWizard = false;

        public MoralisServerSettings()
        {
            DappUrl = String.Empty;
            DappId = String.Empty;
            DappName = "Moralis SDK Application";
            DappDescription = "This application provides an example of how to you Moralis in a Unity 3D Game";
            DappVersion = "1.0.0";
            DappWebsiteUrl = "https://moralis.io";
            DappIconUrl = "https://moralis.io/wp-content/uploads/2021/06/Powered-by-Moralis-Badge-Black.svg";
        }
    }
}