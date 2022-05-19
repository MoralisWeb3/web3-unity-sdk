using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// Renders one dialog within the <see cref="DialogSystem"/>
    /// </summary>
    public class DialogUI : MonoBehaviour
    {
        //  Properties -----------------------------------
        public List<DialogButtonData> DialogButtonDatas { get { return _dialogButtonDatas; } set { _dialogButtonDatas = value; Render(); }}
        public bool IsInteractable { get { return _canvasGroup.interactable; } set { _canvasGroup.interactable = value; }}
        public Text TitleText { get { return _titleText; } }
        public ExampleText BodyText { get { return _bodyText; } }
        private ExampleButton ExampleButtonPrefab { get { return _exampleButtonPrefab; }}
        
        //  Fields ---------------------------------------
        private List<DialogButtonData> _dialogButtonDatas = null;
        
        [Header("Base Fields")]
        [SerializeField]
        private ExampleButton _exampleButtonPrefab;
                
        [SerializeField]
        private Image _backgroundImage = null;

        [SerializeField]
        private Text _titleText = null;

        [SerializeField]
        private ExampleText _bodyText = null;

        [SerializeField]
        private HorizontalLayoutGroup _buttonsHorizontalLayoutGroup = null;
        
        [SerializeField]
        private CanvasGroup _canvasGroup = null;

        //  Unity Methods --------------------------------
        private void OnValidate()
        {
            if (_backgroundImage == null)
            {
                return;
            }

        }

        //  Other Methods --------------------------------
        
        private void Render()
        {
            if ((_dialogButtonDatas == null || _dialogButtonDatas.Count == 0) &&
                _exampleButtonPrefab == null)
            {
                Debug.LogError($"Render() failed. Arguments invalid.");
                return;
            }

            // There may be some some buttons remaining for readability at edit time
            // Clear them out
            _buttonsHorizontalLayoutGroup.transform.ClearChildren();
            
            //Attach only children based on the prefab chosen and the data present
            foreach (DialogButtonData dialogButtonData in _dialogButtonDatas)
            {
                ExampleButton exampleButton = Instantiate(_exampleButtonPrefab, _buttonsHorizontalLayoutGroup.transform);
                exampleButton.transform.SetAsLastSibling();
                exampleButton.Button.onClick.AddListener(dialogButtonData.OnButtonClicked);
                exampleButton.Text.text = dialogButtonData.Text;
            }
        }
    }
}


public static class TransformExtensions
{
    public static void ClearChildren(this Transform trans)
    {
        while (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            child.SetParent((Transform) null);
            Object.DestroyImmediate((Object) child.gameObject);
        }
    }
}
