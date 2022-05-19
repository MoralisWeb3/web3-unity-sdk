using System.Collections.Generic;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExampleFooter : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public ExampleButton Button01 { get { return _buttons[0];}}
      public ExampleButton Button02 { get { return _buttons[1];}}
      public ExampleButton Button03 { get { return _buttons[2];}}
      public CanvasGroup CanvasGroup { get { return _canvasGroup; } }
      
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
      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private List<ExampleButton> _buttons = null;
      
      [SerializeField] 
      private CanvasGroup _canvasGroup = null;

   }
}


