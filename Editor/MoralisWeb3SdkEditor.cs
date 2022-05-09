﻿#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using MoralisUnity.Sdk.Constants;

namespace MoralisUnity.Editor
{
    public class MoralisWeb3SdkEditor : EditorWindow
    {
        private VisualElement rootElement;
        private bool windowDrawn = false;

        public bool isSetupWizard = true;

        protected static Type WindowType = typeof(MoralisWeb3SdkEditor);

        /// <summary>
        /// Evnt handled when the package is installed or the project re-opened.
        /// </summary>
        [UnityEditor.InitializeOnLoadMethod]
        public static void InitializeOnLoadMethod()
        {
            EditorApplication.delayCall += OnDelayCall;
        }

        /// <summary>
        /// Menu show event - displays the setup window when menu selection made.
        /// </summary>
        [MenuItem( MoralisConstants.PathMoralisWindowMenu + "/" + MoralisConstants.Open + " " + "Web3 Setup", false, MoralisConstants.PriorityMoralisWindow_Primary + 5 )]
        public static void ShowWindow()
        {
            var window = GetWindow<MoralisWeb3SdkEditor>();

            window.isSetupWizard = false;
            window.titleContent = new GUIContent("Unity Web3 SDK");
            window.minSize = new Vector2(750, 500);
            window.maxSize = new Vector2(750, 500);
        }

        /// <summary>
        /// Launches the Setupwizard
        /// </summary>
        protected static void ShowSetupWizard()
        {
            MoralisWeb3SdkEditor win = GetWindow(WindowType, false, "Moralis Unity Web3 SDK", true) as MoralisWeb3SdkEditor;
            if (win == null)
            {
                return;
            }

            win.isSetupWizard = true;
            win.minSize = new Vector2(750, 500);
            win.maxSize = new Vector2(750, 500);
            win.Show();
        }

        // Called after OnLoad and displays the setup wizard.
        private static void OnDelayCall()
        {
            if (MoralisSettings.MoralisData == null)
            {
                // Load or (when first run) create the settings scriptable object.
                MoralisSettings.LoadOrCreateSettings(true);
            }

            // If something horrible happened and moralis data setings were not
            // loaded, do not show the wizard.
            if (MoralisSettings.MoralisData == null)
            {
                return;
            }

            // Only show the wizard if it is not disabled and the information has
            // not already been filled in.
            if (!MoralisSettings.MoralisData.DisableAutoOpenWizard && 
                (MoralisSettings.MoralisData.ApplicationId.Equals(String.Empty) ||
                 MoralisSettings.MoralisData.ServerUri.Equals(String.Empty)))
            {
                ShowSetupWizard();
            }
        }

        // Marks settings object as dirty, so it gets saved.
        // unity 5.3 changes the usecase for SetDirty(). but here we don't modify a scene object! so it's ok to use
        private static void SaveSettings()
        {
            EditorUtility.SetDirty(MoralisSettings.MoralisData);
        }

        /// <summary>
        /// Handles the on draw event for the editor window.
        /// </summary>
        protected virtual void OnGUI()
        {
            if (!windowDrawn)
            {
                // Only draw once
                windowDrawn = true;
                string moralisEditorwindowPath = UnityFileHelper.FindMoralisEditorFolder();

                if (MoralisSettings.MoralisData == null)
                {
                    // Just in case moralisData has not been loaded, handle it here.
                    MoralisSettings.LoadOrCreateSettings();
                }

                rootElement = rootVisualElement;

                bool mdLoaded = MoralisSettings.MoralisData != null;

                // Loads the page definition.
                VisualTreeAsset original = AssetDatabase
                    .LoadAssetAtPath<VisualTreeAsset>(moralisEditorwindowPath + "MoralisWeb3SdkEditorWindow.uxml");

                // If page not found, close and exit window
                if (original == null)
                {
                    this.Close();
                    return;
                }

                TemplateContainer treeAsset = original.CloneTree();
                // Add page definition to the root element.
                rootVisualElement.Add(treeAsset);

                // Load stylsheet
                StyleSheet styleSheet = AssetDatabase
                    .LoadAssetAtPath<StyleSheet>(moralisEditorwindowPath + "MoralisWeb3SdkEditorStyles.uss");
                // Apply stylesheet root element.
                rootVisualElement.styleSheets.Add(styleSheet);

                #region Page Button Setup
                // Add action to Save button
                var doneButton = rootVisualElement.Q<Button>("DoneButton");
                doneButton.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    windowDrawn = false;
                    this.Close();
                });
                // Since we start on the first page, back button should start hidden.
                doneButton.style.display = DisplayStyle.None;
                rootElement.Q<Button>("DoneButton").style.display = DisplayStyle.Flex;
                #endregion

                #region TextField Values Setup
                var serverUriField = rootVisualElement.Q<TextField>("ServerUriField");
                serverUriField.SetValueWithoutNotify(MoralisSettings.MoralisData.ServerUri);
                serverUriField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ServerUri = evt.newValue;
                    SaveSettings();
                });

                var applicationIdField = rootVisualElement.Q<TextField>("ApplicationIdField");
                applicationIdField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationId);
                applicationIdField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationId = evt.newValue;
                    SaveSettings();
                });
                #endregion
            }
        }
    }
}
#endif
