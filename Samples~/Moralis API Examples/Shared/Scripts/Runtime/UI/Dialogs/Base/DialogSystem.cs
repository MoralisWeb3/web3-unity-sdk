using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// System which manages show/hide operations for <see cref="DialogUI"/>
    ///
    /// NOTE: This allows for only 0...1 dialog at a time. 
    /// </summary>
    [Serializable]
    public class DialogSystem
    {
        //  Properties ---------------------------------------
        public bool HasCurrentDialogUI { get { return CurrentDialogUI != null; }}
        public DialogUI CurrentDialogUI { get { return _currentDialogUI; }}
        public DialogUI DialogUIPrefabWithText { get { return _dialogUIPrefabWithText; }}
        
        //  Fields ---------------------------------------
        [Header("Other")]
        [SerializeField] 
        private GameObject _dialogParent = null;

        [Header("Prefabs")]
        [SerializeField] 
        private DialogUI _dialogUIPrefabWithText = null;

        private DialogUI _currentDialogUI = null;
        
        //  Constructor ---------------------------------------
        
        //  Other Methods ---------------------------------------
        public T ShowDialogBox<T>(T dialogUIPrefab, 
            string titleText,
            string bodyText,
            List<DialogButtonData> dialogButtonDatas) where  T : DialogUI
        {
            _currentDialogUI = GameObject.Instantiate<T>(dialogUIPrefab, _dialogParent.transform);
            _currentDialogUI.DialogButtonDatas = dialogButtonDatas;
            _currentDialogUI.TitleText.text = titleText;

            // Only some situations need bodyText
            if (!string.IsNullOrEmpty(bodyText))
            {
                if (_currentDialogUI.BodyText.Text != null)
                {
                    _currentDialogUI.BodyText.SetTextSafe(bodyText);
                    _currentDialogUI.BodyText.ScrollToTop();
                }
                else
                {
                    Debug.LogError("NullRef. Either pass bodyText='' or use prefab with BodyText.text.");
                }
            }
       
            return _currentDialogUI as T;
        }

        
        public void HideDialogBox()
        {
            if (_currentDialogUI != null)
            {
                GameObject.Destroy(_currentDialogUI.gameObject);
            }

            _currentDialogUI = null;
        }
        
        /// <summary>
        /// Shows 'are you sure?". Has ok/cancel
        /// </summary>
        public DialogUI ShowDialogBoxConfirmation(Action confirmationAction, Action cancelAction)
        {
            return ShowDialogBox<DialogUI>(
                _dialogUIPrefabWithText,
                ExampleConstants.DialogConfirmation,
                ExampleConstants.DialogAreYouSure,
                new List<DialogButtonData>
                {
                    new DialogButtonData(ExampleConstants.Ok, delegate
                    {
                        confirmationAction.Invoke();
                        HideDialogBox();
                    }),
                    new DialogButtonData(ExampleConstants.Cancel, delegate
                    {
                        cancelAction.Invoke();
                        HideDialogBox();
                    })
                });
        }
        
        /// <summary>
        /// Shows custom title/body. Has ok/cancel buttons
        /// </summary>
        public DialogUI ShowDialogBoxCustomText(string titleText, string bodyText,
            Action confirmationAction, Action cancelAction)
        {
            if (HasCurrentDialogUI)
            {
                throw new Exception("Must call HideDialogBox() or equivalent, before ShowDialogBoxLoading().");
            }
            
            DialogUI dialogUI = ShowDialogBox<DialogUI>(
                _dialogUIPrefabWithText,
                titleText,
                bodyText,
                new List<DialogButtonData>
                {
                    new DialogButtonData(ExampleConstants.Ok, delegate
                    {
                        confirmationAction.Invoke();
                        HideDialogBox();
                    }),
                    new DialogButtonData(ExampleConstants.Cancel, delegate
                    {
                        cancelAction.Invoke();
                        HideDialogBox();
                    })
                });
            
            return dialogUI;
        }
        
        /// <summary>
        /// Shows 'loading' and has no buttons
        /// </summary>
        public DialogUI ShowDialogBoxLoading(string titleOfLoadingContent)
        {
            if (HasCurrentDialogUI)
            {
                throw new Exception("Must call HideDialogBox() or equivalent, before ShowDialogBoxLoading().");
            }
            
            DialogUI dialogUI = ShowDialogBox<DialogUI>(
                _dialogUIPrefabWithText,
                string.Format(ExampleConstants.DialogLoading, titleOfLoadingContent),
                "",
                new List<DialogButtonData>());
            
            return dialogUI;
        }
    }
}