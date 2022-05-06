
#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using MoralisUnity.Sdk.Constants;

namespace MoralisUnity.Editor
{
    public class Web3SettingsEditor : EditorWindow
    {
        private static string[] pages = { "Page_1", "Page_2" };
        
        private VisualElement rootElement;
        private bool windowDrawn = false;

        public bool isSetupWizard = true;

        protected static Type WindowType = typeof(Web3SettingsEditor);



        /// <summary>
        /// Menu show event - displays the setup window when menu selection made.
        /// </summary>
        [MenuItem(MoralisConstants.PathMoralisWindowMenu + "/" + MoralisConstants.Open + " " + "Web3 Request Settings", false, 5 )]
        public static void ShowWindow()
        {
            var window = GetWindow<Web3SettingsEditor>();

            window.isSetupWizard = false;
            window.titleContent = new GUIContent("Web3 Request Settings");
            window.minSize = new Vector2(750, 500);
            window.maxSize = new Vector2(750, 500);

            window.Show();
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
                    .LoadAssetAtPath<VisualTreeAsset>(moralisEditorwindowPath + "Web3SettingsEditorWindow.uxml");

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
                #endregion

                #region TextField Values Setup
                var applicationNameField = rootVisualElement.Q<TextField>("ApplicationNameField");
                applicationNameField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationName);
                applicationNameField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationName = evt.newValue;
                    SaveSettings();
                });

                var applicationDescriptionField = rootVisualElement.Q<TextField>("ApplicationDescriptionField");
                applicationDescriptionField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationDescription);
                applicationDescriptionField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationDescription = evt.newValue;
                    SaveSettings();
                });


                var applicationVersionField = rootVisualElement.Q<TextField>("ApplicationVersionField");
                applicationVersionField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationVersion);
                applicationVersionField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationVersion = evt.newValue;
                    SaveSettings();
                });

                var applicationUrlField = rootVisualElement.Q<TextField>("ApplicationUrlField");
                applicationUrlField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationUrl);
                applicationUrlField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationUrl = evt.newValue;
                    SaveSettings();
                });

                var applicationIconUriField = rootVisualElement.Q<TextField>("ApplicationIconUriField");
                applicationIconUriField.SetValueWithoutNotify(MoralisSettings.MoralisData.ApplicationIconUri);
                applicationIconUriField.RegisterValueChangedCallback(evt =>
                {
                    MoralisSettings.MoralisData.ApplicationIconUri = evt.newValue;
                    SaveSettings();
                });
                #endregion

            }
        }
    }
}
#endif