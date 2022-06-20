using Assets.Scripts;
using Assets.Scripts.WalletConnectSharp.Unity.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

namespace WalletConnectSharp.Unity.UI
{
    public class ChooseWalletScreen : MonoBehaviour
    {
        public WalletConnect WalletConnect;
        public Transform buttonGridTransform;
        public Text loadingText;
        public Text statusText;
        
        [SerializeField]
        public WalletSelectItem[] wallets;

        private bool walletButtonsCreated = false;

        private void Start()
        {
#if UNITY_IOS
            // Set wallet filter to those wallets selected by the developer.
            IEnumerable<string> walletFilter = from w in wallets
                                               where w.Selected == true
                                               select w.Name;
            // For iOS Set wallet filter to speed up wallet button display.
            if (walletFilter.Count() > 0)
            {
                WalletConnect.AllowedWalletNames = walletFilter.ToList();
            }
            else
            {
                Debug.Log("No wallets selected for filter.");
            }
#endif              
            //StartCoroutine(BuildWalletButtons());
            //BuildWalletButtons();
        }

        private void Update()
        {
            if (!walletButtonsCreated && WalletConnect.SupportedWallets != null && WalletConnect.SupportedWallets.Count > 1)
            {
                walletButtonsCreated = true;
                StartCoroutine(BuildWalletButtons());
            }
        }

        private IEnumerator BuildWalletButtons()
        {
            yield return WalletConnect.FetchWalletList();

            Debug.Log("Building wallet buttons.");
            
            GameObject buttonPrefab = new GameObject("buttonPrefab");
            buttonPrefab.AddComponent<Image>();
            buttonPrefab.AddComponent<Button>();

            foreach (var walletId in WalletConnect.SupportedWallets.Keys)
            {
                var walletData = WalletConnect.SupportedWallets[walletId];

                var walletObj = Instantiate(buttonPrefab, buttonGridTransform);

                var walletImage = walletObj.GetComponent<Image>();
                var walletButton = walletObj.GetComponent<Button>();

                walletImage.sprite = walletData.medimumIcon;
                
                walletButton.onClick.AddListener(delegate
                {
                    // Check if WalletConnect is ready for the user prompt to make sure the user wont get stuck
                    if (WalletConnect.Session.ReadyForUserPrompt)
                    {
                        WalletConnect.OpenDeepLink(walletData);
                        // hide wallets after 
                        gameObject.SetActive(false);
                        // show status text
                        statusText.gameObject.SetActive(true);
                    }
                });
            }
            
            Destroy(loadingText.gameObject);
        }

        public static List<WalletSelectItem> GetWalletNameList()
        {
            List<WalletSelectItem> result = SupportedWalletList.SupportedWalletNames();

            return result;
        }
    }
}