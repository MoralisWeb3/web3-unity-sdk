using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// The UI for shared use.
   /// </summary>
   public class ExamplePanel : MonoBehaviour
   {
      //  Properties  ---------------------------------------
      public Text TitleText
      {
         get
         {
            return _titleText;
         }
         set
         {
            _titleText = value;
         }
      }
      
      public ExampleText BodyText
      {
         get
         {
            return _bodyText;
         }
         set
         {
            _bodyText = value;
         }
      }

      public ExampleButton CopyButton { get { return _copyButton; } }
      public CanvasGroup CanvasGroup { get { return _canvasGroup; } }
      
      public LayoutElement LayoutElement { get { return _layoutElement; } }
      
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

      [Header("UI")]
      [SerializeField] 
      private Text _titleText = null;
      
      [SerializeField] 
      private ExampleText _bodyText = null;

      [SerializeField] 
      private ExampleButton _copyButton = null;

      [SerializeField] 
      private CanvasGroup _canvasGroup = null;

      [SerializeField] 
      private LayoutElement _layoutElement = null;

      [Header("Settings")]
      [SerializeField] 
      private bool _isCopyButtonVisible = true;

      
      //  Unity Methods ---------------------------------	
      protected void Awake()
      {
         _copyButton.Button.onClick.AddListener(CopyButton_OnClicked);
         _copyButton.gameObject.SetActive(_isCopyButtonVisible);
      }
      
      //  General Methods -------------------------------	
      
      //  Event Handlers --------------------------------
      private void CopyButton_OnClicked()
      {
         GUIUtility.systemCopyBuffer = _bodyText.Text.text;
      }
   }
}


