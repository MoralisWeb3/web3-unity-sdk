using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExampleButton : MonoBehaviour
   {
      
      //  Properties  ---------------------------------------
      public Text Text { get { return _text;}}
      public Button Button { get { return _button;}}

      public bool IsVisible
      {
         get
         {
            return _canvasGroup.GetIsVisible();
         }
         set
         {
            _canvasGroup.SetIsVisible(value);
         }
      }
      
      public bool IsInteractable
      {
         get
         {
            return _canvasGroup.interactable;
         }
         set
         {
            _canvasGroup.interactable = value;
         }
      }

      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private Button _button = null;
      
      [SerializeField] 
      private CanvasGroup _canvasGroup = null;
      
      [SerializeField] 
      private Text _text = null;
      
      //  Unity Methods  --------------------------------
   }
}


