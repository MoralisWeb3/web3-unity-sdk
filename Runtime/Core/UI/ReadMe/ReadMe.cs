using System;
using UnityEngine;
using MoralisUnity.Sdk.Constants;

namespace MoralisUnity.Sdk.UI.ReadMe
{
    /// <summary>
    /// Custom-formatted readme file with markdown-like display. 
    /// 
    /// Inspired by Unity's "Learn" Sample Projects
    /// 
    /// </summary>
    public class ReadMe : ScriptableObject
    {
        private const string Title = "ReadMe";
        public Texture2D icon;
        public string title;
        public Section[] sections;
        public bool loadedLayout;

        [Serializable]
        public class Section
        {
            public string heading, subheading, text, linkText, url, pingObjectName, pingObjectGuid, 
                openMenuName, openMenuNameId;
        }
    }
}