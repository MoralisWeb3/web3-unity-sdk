using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// Reusable UI for rendering one dropdown menu.
   /// </summary>
   public class ExampleDropdown : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public Dropdown Dropdown { get { return _dropdown;}}
      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private Dropdown _dropdown = null;
      
      [SerializeField] 
      private CanvasGroup _canvasGroup = null;
      
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
      
      //  Unity Methods  --------------------------------
   }
}


