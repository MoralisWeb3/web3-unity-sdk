using System;
using UnityEngine;

namespace MoralisUnity.Sdk.UI
{
    /// <summary>
    /// Custom-formatted readme file with markdown-like display. 
    /// 
    /// Inspired by Unity's "Learn" Sample Projects
    /// 
    /// </summary>
    [CreateAssetMenu(
        fileName = Title,
        menuName = CreateAssetMenu + "/" + Title,
        order = 0)]
    public class ReadMe : ScriptableObject
    {
        //TODO: Move this to const file afterwards, if/when moving into SDK
        public const string CreateAssetMenu = "Moralis/Examples";
        
        private const string Title = "ReadMe";
        public Texture2D icon;
        public string title;
        public Section[] sections;
        public bool loadedLayout;

        [Serializable]
        public class Section
        {
            public string heading, subheading, text, linkText, url, pingObjectName, pingObjectPath, 
                openMenuName, openMenuNameId;
        }
    }
}