using UnityEngine.Events;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// Defines each <see cref="Button"/> in the <see cref="DialogSystem"/>
    /// </summary>
    public class DialogButtonData
    {
        //  Properties ---------------------------------------
        public string Text { get { return _text;  }}
        public UnityAction OnButtonClicked { get { return _onButtonClicked; }}
        
        //  Fields ---------------------------------------
        private readonly string _text = "";
        private readonly UnityAction _onButtonClicked = null;

        //  Constructor ---------------------------------------

        public DialogButtonData(string text, UnityAction onButtonClicked)
        {
            _text = text;
            _onButtonClicked = onButtonClicked;
        }
        
        //  Other Methods ---------------------------------------
    }
}