#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

namespace Moralis.Web3UnitySdk.Editor
{
    public class MoralisWeb3SdkEditor : EditorWindow
    {
        private static string[] pages = { "Page_1", "Page_2" };
        private static string ServerUriDefaultText = "SERVER URI";
        private static string ApplicationIdDefaultText = "APPLICATION ID";

        private int currentPageIndex = 0;
        private VisualElement rootElement;
        private bool windowDrawn = false;

        public bool isSetupWizard = true;

        protected static Type WindowType = typeof(MoralisWeb3SdkEditor);

        /// <summary>
        /// Based on new page vs current, adjust page shown and which buttons are shown.
        /// </summary>
        /// <param name="newPageIndex"></param>
        private void AdjustPageAndButtons(int newPageIndex)
        {
            // If page has changed hide current page, and show new page.
            if (newPageIndex != currentPageIndex)
            {
                rootElement.Q(pages[currentPageIndex]).style.display = DisplayStyle.None;
                rootElement.Q(pages[newPageIndex]).style.display = DisplayStyle.Flex;

                currentPageIndex = newPageIndex;
            }

            // If on last page hide Next button
            if (currentPageIndex >= (pages.Length - 1))
            {
                rootElement.Q<Button>("NextButton").style.display = DisplayStyle.None;
                rootElement.Q<Button>("SkipButton").style.display = DisplayStyle.None;
                rootElement.Q<Button>("DoneButton").style.display = DisplayStyle.Flex;
            }
            else
            {
                rootElement.Q<Button>("NextButton").style.display = DisplayStyle.Flex;
                rootElement.Q<Button>("SkipButton").style.display = DisplayStyle.Flex;
                rootElement.Q<Button>("DoneButton").style.display = DisplayStyle.None;
            }

            // If on first page hide Back button.
            if (currentPageIndex < 1)
            {
                rootElement.Q<Button>("BackButton").style.display = DisplayStyle.None;
            }
            else
            {
                rootElement.Q<Button>("BackButton").style.display = DisplayStyle.Flex;
            }
        }

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
        [MenuItem("Window/Moralis/Web3 Unity SDK/Open Web3 Unity SDK Setup %#&m", false, 0)]
        public static void ShowWindow()
        {
            var window = GetWindow<MoralisWeb3SdkEditor>();

            window.isSetupWizard = false;
            window.titleContent = new GUIContent("Unity Web3 SDK");
            window.minSize = new Vector2(750, 375);
            window.maxSize = new Vector2(750, 375);
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
                Debug.LogError("Could not load MoralisDataScriptableObject");

                return;
            }

            // Only show the wizard if it is not disabled and the information has
            // not already been filled in.
            if (!MoralisSettings.MoralisData.DisableAutoOpenWizard && 
                (MoralisSettings.MoralisData.ApplicationId.Equals(ApplicationIdDefaultText) ||
                 MoralisSettings.MoralisData.ServerUri.Equals(ServerUriDefaultText)))
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

                if (isSetupWizard)
                {
                    currentPageIndex = 0;
                }
                else
                {
                    currentPageIndex = 1;
                }

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
                // Add action to Back button
                var backButton = rootVisualElement.Q<Button>("BackButton");
                backButton.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    int newPageIndex = currentPageIndex;

                    newPageIndex--;

                    // If new index is less than zero, stay on the current
                    // view.
                    if (newPageIndex < 0)
                    {
                        newPageIndex = currentPageIndex;
                    }

                    // If page has changed, update the view.
                    if (newPageIndex != currentPageIndex)
                    {
                        AdjustPageAndButtons(newPageIndex);
                    }
                });

                // Since we start on the first page, back button should start hidden.
                backButton.style.display = DisplayStyle.None;

                // Add action to Next Button
                var nextButton = rootVisualElement.Q<Button>("NextButton");
                nextButton.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    int newPageIndex = currentPageIndex;

                    newPageIndex++;

                    // If the new page index is greater than the number of
                    // pages, stay on current page.
                    if (newPageIndex >= pages.Length)
                    {
                        newPageIndex = currentPageIndex;
                    }

                    // If page has changed, update the view.
                    if (newPageIndex != currentPageIndex)
                    {
                        AdjustPageAndButtons(newPageIndex);
                    }
                });

                // Add action to Save button
                var doneButton = rootVisualElement.Q<Button>("DoneButton");
                doneButton.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    windowDrawn = false;
                    this.Close();
                });
                // Since we start on the first page, back button should start hidden.
                doneButton.style.display = DisplayStyle.None;

                // Add action to Skip button
                var skipButton = rootVisualElement.Q<Button>("SkipButton");
                skipButton.RegisterCallback<MouseUpEvent>((evt) =>
                {
                    windowDrawn = false;
                    MoralisSettings.MoralisData.DisableAutoOpenWizard = true;
                    this.Close();
                });
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

                if (pages.Length > 0)
                {
                    for (int i = 0; i < pages.Length; i++)
                    {
                        if (i != currentPageIndex)
                        {
                            rootVisualElement.Q(pages[i]).style.display = DisplayStyle.None;
                        }
                    }

                    AdjustPageAndButtons(currentPageIndex);
                }
            }
        }
    }
}
#endif
