#if UNITY_EDITOR
/**
*           Module: UnityFileHelper.cs
*  Descriptiontion: Class that provides some functions for working with 
*                   files in a Unity project.
*           Author: Moralis Web3 Technology AB, 559307-5988 - David B. Goodrich
*  
*  MIT License
*  
*  Copyright (c) 2022 Moralis Web3 Technology AB, 559307-5988
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
using System.IO;
using UnityEditor;

namespace MoralisUnity
{
    public class UnityFileHelper
    {
        /// <summary>
        /// Finds the asset path base on its name or search query: https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
        /// </summary>
        /// <returns>The asset path.</returns>
        /// <param name="asset">Asset.</param>
        public static string FindAssetPath(string asset, string targetFileName=null)
        {
            string[] guids = AssetDatabase.FindAssets(asset, null);

            if (guids.Length != 1)
            {
                if (guids.Length > 1 && targetFileName != null)
                {
                    foreach (string g in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(g);

                        if (Path.GetFileName(path).Equals(targetFileName))
                        {
                            return path;
                        }
                    }
                }

                return string.Empty;
            }
            else
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return path;
            }
        }

        /// <summary>
        /// Finds the Moralis asset folder. Something like Assets/Moralis Web3 Unity SDK/Resources/
        /// </summary>
        /// <returns>The Moralis asset folder.</returns>
        public static string FindMoralisAssetFolder()
        {
            string path = FindPathFromAsset("MoralisServerSettings", "Moralis Web3 Unity SDK");

            if (String.IsNullOrEmpty(path))
            {
                return "Assets/Moralis Web3 Unity SDK/Resources/";
            }
            else 
            {
                return path;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FindMoralisEditorFolder()
        {
            string path = FindPathFromAsset("MoralisWeb3SdkEditor", "Editor", "MoralisWeb3SdkEditor.cs");

            if (String.IsNullOrEmpty(path))
            {
                return "Assets/Moralis Web3 Unity SDK/Resources/";
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Finds a specific target folder in a path that the specified asset is located in.
        /// </summary>
        /// <param name="assetPathName"></param>
        /// <param name="targetSubFolder"></param>
        /// <returns></returns>
        private static string FindPathFromAsset(string assetPathName, string targetSubFolder, string targetFileName=null)
        {
            string _thisPath = FindAssetPath(assetPathName, targetFileName);
            string _targetFolderPath = string.Empty;

            string[] subdirectoryEntries = _thisPath.Split('/');

            foreach (string dir in subdirectoryEntries)
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    _targetFolderPath += dir + "/";

                    if (string.Equals(dir, targetSubFolder))
                    {
                        return _targetFolderPath;
                    }
                }
            }

            return null;
        }
    }
}
#endif