using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExampleImage : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public bool IsVisible
      {
         get
         {
            return _image.enabled;
         }
         set
         {
            _image.enabled = value;
         }
      }
      

      //  Fields  ---------------------------------------
      
      [SerializeField] 
      private Image _image = null;

   }
}
