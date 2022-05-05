using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExampleHeader : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public Text TitleText {  get { return _titleText; } set  {  _titleText = value; } }
      public ChainsDropdown ChainsDropdown { get { return _chainsDropdown; } }
      public ExampleAuthenticationUI AuthenticationUI { get { return _authenticationUI; } }
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
      private Text _titleText = null;
      
      [SerializeField] 
      private ChainsDropdown _chainsDropdown = null;

      [SerializeField] 
      private ExampleAuthenticationUI _authenticationUI = null;

      [SerializeField] 
      private CanvasGroup _canvasGroup = null;
      
   }
}
