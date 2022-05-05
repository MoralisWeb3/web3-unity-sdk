
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// Helper methods for this class
    /// </summary>
    public static class CanvasGroupExtensions
    {
        //  General Methods  --------------------------------------
        public static void SetIsVisible(this CanvasGroup canvasGroup, bool isVisible)
        {
            if (isVisible)
            {
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
        

        public static bool GetIsVisible(this CanvasGroup canvasGroup)
        {
            return Mathf.Approximately(canvasGroup.alpha, 1);
        }
        
        public static void SetIsInteractable(this CanvasGroup canvasGroup, bool isInteractable)
        {
            canvasGroup.interactable = isInteractable;
        }

        public static bool GetIsInteractable(this CanvasGroup canvasGroup)
        {
            return canvasGroup.interactable == true;
        }

    }
}