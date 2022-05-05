using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WalletConnectSharp.Unity;
using Task = System.Threading.Tasks.Task;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// This helper hides complex-but-required code
    /// which is outside the educational scope of per-scene
    /// example files
    /// </summary>
    public static class ExampleHelper
    {
        //  General Methods  --------------------------------------
        
        /// <summary>
        /// Convert from visuals into string
        /// </summary>
        public static string ConvertSpriteToContentString(Sprite sprite)
        {
            Texture2D mytexture = sprite.texture;
            byte[] bytes;
            bytes = mytexture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }
        
        
        /// <summary>
        /// Convert from URl of string into visuals
        /// </summary>
        public static async UniTask<Sprite> CreateSpriteFromImageUrl(string path)
        {   
            var getRequest = UnityWebRequest.Get(path);
            await getRequest.SendWebRequest();

            Texture2D textured2D = new Texture2D(2, 2);
            byte[] bytes = getRequest.downloadHandler.data;

            // Empty bytes means empty image. Support that.
            if (bytes != null && bytes.Length > 0)
            { 
                textured2D.LoadImage(bytes);
            }
            
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(textured2D, new Rect(0.0f, 0.0f, textured2D.width, textured2D.height), pivot, 100.0f);
            return sprite;
        }


        public static Image CreateNewImageUnderParentAsLastSibling(Transform parent)
        {
            GameObject imageGo = new GameObject("NewImage");
            LayoutElement layoutElement = imageGo.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 50;
            Image image = imageGo.AddComponent<Image>();
            image.preserveAspect = true;
            imageGo.transform.SetParent(parent);
            imageGo.transform.SetSiblingIndex(imageGo.transform.childCount-1);
            CanvasGroup canvasGroup = imageGo.AddComponent<CanvasGroup>();
            return image;
        }

        
        public static List<string> ConvertTextAssetToLines(TextAsset textAsset, int startLineIndex = 0)
        {
            var separator = new string[] { "\r\n", "\r", "\n" };
            var sourceLines = textAsset.text.Split(separator, StringSplitOptions.None).ToList();
            List<string> destinationLines = new List<string>(); 
            for (int l = startLineIndex; l < sourceLines.Count; l++)
            {
                destinationLines.Add(sourceLines[l]);
            }

            return destinationLines;
        }

        
        /// <summary>
        /// Ideally the canvas automatically adjusts purely based on the visible text.
        /// That is doable. However, this is a quicker hack.
        /// </summary>
        public static void SetExamplePanelPreferredHeight(ExamplePanel examplePanel, float panelHeight)
        {
            examplePanel.LayoutElement.preferredHeight = panelHeight;
        }
        
        
        public static Vector2 GetExamplePanelActualSize(ExamplePanel examplePanel, Canvas canvas)
        {
	        RectTransform rectTransform = examplePanel.transform.parent.gameObject.GetComponent<RectTransform>();
	        return RectTransformUtility.PixelAdjustRect(rectTransform, canvas).size;
        }
        
        
        /// <summary>
        /// Wait X seconds so user sees the noticeable feedback
        /// flicker of "Loading..." in the text box
        /// </summary>
        public static async UniTask TaskDelayWaitForCosmeticEffect()
        {
            await Task.Delay(100);
        }

        /// <summary>
        /// Wait 1 frame for UI to render
        /// </summary>
        public static async UniTask TaskDelayWaitForEndOfFrame2()
        {
            await UniTask.WaitForEndOfFrame();
        }
        
        /// <summary>
        /// Changes "blah_blah" to "Blah Blah"
        /// </summary>
        /// <param name="chainEntry"></param>
        /// <returns></returns>
        public static string GetPrettifiedNameByChainEntry(ChainEntry chainEntry)
        {
	        string name = chainEntry.Name.Replace("_", " ");
	        name = ToTitleCase(name);
	        return name;
        }

        
        private static string ToTitleCase(string message)
        {
	        TextInfo myTI = new CultureInfo("en-US",false).TextInfo;
	        return myTI.ToTitleCase(message);
        }

        
        /// <summary>
        /// Call Moralis Servers and get the current server time.
        /// This is used for specific use-cases including EthSign.
        /// </summary>
        /// <returns></returns>
        public static async UniTask<long> GetServerTime()
        {
            long serverTime = 0;
            // Get Server Time (Needed for EthSign)
            Dictionary<string, object> serverTimeResponse = 
                await Moralis.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());
            if (serverTimeResponse == null || 
                !serverTimeResponse.ContainsKey("dateTime") ||
                !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
            {
                Debug.Log("Failed to retrieve server time from Moralis Server!");
            }

            return serverTime;
        }


        public static string GetSceneAssetName(SceneAsset sceneAsset)
        {
            string scenePath =  GetSceneAssetPath(sceneAsset);
            string sceneName = string.Empty;
            var sceneObj = AssetDatabase.LoadMainAssetAtPath(scenePath);
            if (sceneObj != null)
            {
                sceneName = sceneObj.name;
            }

            return sceneName;

        }
        
        
        public static string GetSceneAssetPath(SceneAsset sceneAsset)
        {
            string scenePath = string.Empty;
            if (sceneAsset != null)
            {
                scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            }
            return scenePath;
        }

        /// <summary>
        /// Send a simple sign request.
        /// 
        /// <see cref="https://www.codementor.io/@yosriady/signing-and-verifying-ethereum-signatures-vhe8ro3h6#background"/>
        /// <see cref="https://support.mycrypto.com/how-to/getting-started/how-to-sign-and-verify-messages-on-ethereum/"/>
        /// <see cref="https://docs.moralis.io/misc/faq#why-do-you-use-the-signing-messages-and-other-dapps-dont"/>
        /// </summary>
        /// <param name="walletConnect"></param>
        /// <param name="address"></param>
        /// <param name="message"></param>
        public static async UniTask<string> Sign(WalletConnect walletConnect, 
            string address, string message)
        {
#if UNITY_WEBGL
            return await Web3GL.Sign(myKnownMessage);
#else
            return await walletConnect.Session.EthPersonalSign(address, message);
#endif
        }

    }
}