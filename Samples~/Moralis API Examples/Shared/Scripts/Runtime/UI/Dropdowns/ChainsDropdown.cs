
using MoralisUnity;
using MoralisUnity.Web3Api.Models;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// Non-Reusable, custom implementation of the reusable
   /// <see cref="ExampleDropdown"/>
   /// </summary>
   public class ChainsDropdown : ExampleDropdown
   {
      //  Events  ----------------------------------------
      public ChainEntryUnityEvent OnValueChanged = new ChainEntryUnityEvent();

      //  Properties  -----------------------------------
      
      //  Fields  ---------------------------------------
      
      //  Unity Methods  --------------------------------
      protected void Start()
      {
         Dropdown.onValueChanged.AddListener(Dropdown_OnValueChanged);
      }

      //  General Methods  ------------------------------
      public void SetSelectedChain(ChainList chainList)
      {
         int chainId = SupportedEvmChains.FromChainList(chainList).ChainId;
         Dropdown.value = Dropdown.options.FindIndex(optionData =>
         {
            return (optionData as ChainEntryDropdownOptionData).ChainEntry.ChainId == chainId;
         });
      }
      
      public ChainList GetSelectedChain()
      {
         return GetSelectedChainEntry().EnumValue;
      }
      
      private ChainEntry GetSelectedChainEntry()
      {
         return (Dropdown.options[Dropdown.value] as ChainEntryDropdownOptionData).ChainEntry;
      }
      
      //  Event Handlers --------------------------------
      private void Dropdown_OnValueChanged(int value)
      {
         
         OnValueChanged.Invoke(GetSelectedChainEntry());
      }
   }
}


