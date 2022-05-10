using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Sdk.DesignPatterns.Creational.Singleton.SingletonMonobehaviour;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// Example local storage which is written to disk
   /// and used throughout various examples to hold shared state.
   /// </summary>
   public class ExampleLocalStorage : SingletonMonobehaviour<ExampleLocalStorage>
   {

      //  Events  ---------------------------------------
      [HideInInspector]
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
         }
      }

      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private string _activeAddress = "";
      
      [SerializeField] 
      private string _sceneNamePrevious = "";

      
      //  Unity Methods  --------------------------------
      
      
      //  General Methods  ------------------------------
      public override void InstantiateCompleted()
      {
         //Debug.Log("InstantiateCompleted()");
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
         if (Moralis.IsLoggedIn())
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


