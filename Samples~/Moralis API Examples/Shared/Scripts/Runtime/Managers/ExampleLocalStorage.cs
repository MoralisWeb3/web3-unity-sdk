using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;
using UnityEditor;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// Example local storage which is written to disk
   /// and used throughout various examples to hold shared state.
   /// </summary>
   [FilePath(ExampleConstants.ExamplePreferencesPath + "ExampleLocalStorage.json", FilePathAttribute.Location.PreferencesFolder)]
   public class ExampleLocalStorage: ScriptableSingleton<ExampleLocalStorage>
   {
      //  Events  ---------------------------------------
      public StringUnityEvent OnActiveAddressChanged = new StringUnityEvent();
      
      //  Properties  -----------------------------------
      public bool HasActiveAddress { get { return !string.IsNullOrEmpty(_activeAddress);}}
      public string ActiveAddress
      {
         get
         {
            return _activeAddress;
         }
         set
         {
            
            bool isChanging = _activeAddress != value;
            _activeAddress = value;
            if (isChanging)
            {
               Save(true);
            }
            OnActiveAddressChanged.Invoke(_activeAddress);
         }
      }
      
      public bool HasSceneNamePrevious { get { return !string.IsNullOrEmpty(SceneNamePrevious);}}
      public string SceneNamePrevious
      {
         get
         {
            return _sceneNamePrevious;
         }
         set
         {
            bool isChanging = _sceneNamePrevious != value;
            _sceneNamePrevious = value;
            if (isChanging)
            {
               Save(true);
            }
         }
      }

      //  Fields  ---------------------------------------
      [SerializeField] 
      private string _activeAddress = "";
      
      [SerializeField] 
      private string _sceneNamePrevious = "";

      //  Unity Methods  --------------------------------
      
      //  General Methods  ------------------------------
      
      /// <summary>
      /// Wrapper for API that may change soon
      /// </summary>
      /// <returns></returns>
      public bool MoralisInterfaceIsLoggedIn()
      {
         return  Moralis.IsLoggedIn();
      }
      
      
      public void ResetSceneNamePrevious()
      {
         SceneNamePrevious = string.Empty;
      }
      
      
      public async UniTask<string> ResetActiveAddress()
      {
         MoralisUser moralisUser = await Moralis.GetUserAsync();
         
         // The default experience is...
         // To be logged in...
         // with active address set to the original
         if (MoralisInterfaceIsLoggedIn())
         {
            ActiveAddress = moralisUser.ethAddress;
         }
         else
         {
            ActiveAddress = string.Empty;
         }

         return ActiveAddress;
      }
   }
}


