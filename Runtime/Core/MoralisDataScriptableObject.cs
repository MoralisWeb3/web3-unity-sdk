using System;
using UnityEngine;

/// <summary>
/// Provides a persistable object to hold Moralis related data.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "MorlaisData", menuName = "ScriptableObjects/MoralisDataScriptableObject", order = 1)]
public class MoralisDataScriptableObject : ScriptableObject
{
    public string ServerUri;
    public string ApplicationId;
    public string ApplicationName;
    public string ApplicationDescription;
    public string ApplicationVersion;
    public string ApplicationIconUri;
    public string ApplicationUrl;
    public bool DisableAutoOpenWizard = false;

    public MoralisDataScriptableObject()
    {
        ServerUri = "SERVER URI";
        ApplicationId = "APPLICATION ID";
        ApplicationName = "Moralis SDK Application";
        ApplicationDescription = "This application provdies an example of how to you Moralis in a Unity 3D Game";
        ApplicationVersion = "1.0.0";
        ApplicationUrl = "https://moralis.io";
        ApplicationIconUri = "https://moralis.io/wp-content/uploads/2021/06/Powered-by-Moralis-Badge-Black.svg";
    }
}