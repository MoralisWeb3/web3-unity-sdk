using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExampleText : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public Text Text { get { return _text;}}
      public InputField InputField { get { return _inputField;}}
      public bool IsEditable { get { return _isEditable;}}
      
      public void ScrollToTop()
      {
         StartCoroutine(ScrollToTop_Coroutine());
      }
      
      public void ScrollToBottom()
      {
         StartCoroutine(ScrollToBottom_Coroutine());
      }
      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private ScrollRect _scrollRect = null;
      
      [SerializeField] 
      private Text _text = null;

      [SerializeField] 
      private InputField _inputField = null;

      [SerializeField] 
      private bool _isEditable = false;
      
      //  General Methods  --------------------------------
      private IEnumerator ScrollToTop_Coroutine()
      {
         // Called after delay for desired result
         _scrollRect.ScrollToTop();
         yield return new WaitForEndOfFrame();
         _scrollRect.ScrollToTop();
      }
      
      private IEnumerator ScrollToBottom_Coroutine()
      {
         // Called after delay for desired result
         _scrollRect.ScrollToBottom();
         yield return new WaitForEndOfFrame();
         _scrollRect.ScrollToBottom();
      }
      
      /// <summary>
      /// Sets one of several values depending on context
      /// </summary>
      /// <param name="text"></param>
      public void SetTextSafe(string text)
      {
         if (IsEditable)
         {
            InputField.text = text; 
         }
         else
         {
            Text.text = text; 
         }
      }

     

   }
}


