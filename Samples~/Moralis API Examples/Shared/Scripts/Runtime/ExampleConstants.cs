

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// </summary>
    public static class ExampleConstants
    {
        //  Fields  -----------------------------------------------
        public const string Moralis = "Moralis";
        public const string Chains = "Chains";
        public const string Main = "Main";
        public const string Details = "Details";
        public const string CreateAssetMenu = "Moralis/Examples";
        public const string Loading = "Loading ...";
        public const string NothingAvailable = "Nothing available";
        public const string Authenticate = "Authenticate";
        public const string Results = "Results";
        public const string Type = "Type";
        public const string LogOut = "Log Out";
        public const string NotExpectedSoFix = "Not Expected. Fix.";
        public const string CloudFunctionNotFound = "Empty result. Ensure Cloud Function exists on server.";
        public const string TopPanelBodyTextMustLogInFirst1 = "Load the '{0}' Scene to Log In. Then return to the '{1}' Scene to continue.\n";
        public const string YouAreNotLoggedIn = "You are not logged in.";
        public static readonly string TopPanelBodyTextMustLogInFirst2 = $"Or click '{Authenticate}' above.";
        //
        public const string DialogConfirmation = "Confirmation";
        public const string DialogAreYouSure = "Are you sure?";
        public const string DialogReset = "Reset";

        public const string DialogLoading = "Loading...";
        public const string DialogTitleTextAuthenticate = "Authentication";
        public static readonly string DialogBodyTextAuthenticate = $"Click '{Ok}' to {TopPanelBodyTextMustLogInFirst1}";
        public const string DialogTitleAddress = "Address";
        //
        public const string Ok = "Ok";
        public const string Cancel = "Cancel";

        // Urls
        public const string MoralisServersUrl = "https://admin.moralis.io/servers#";

        // A random account (not mine) with much history for testing - https://etherscan.io/address/0xda9dfa130df4de4673b89022ee50ff26f6ea73cf
        public const string AddressForTesting = "0xDA9dfA130Df4dE4673b89022EE50ff26f6EA73Cf";
        public const string TokenAddressForTesting = "0x00000000219ab540356cbb839cbe05303d7705fa";
        
        //Compiled contract by Moralis via Remix
        public const string AddressForContractTesting = "0x698d7D745B7F5d8EF4fdB59CeB660050b3599AC3";
        public static string SceneSetupInstructions = "Scene Setup Instructions";

        public static float GetBottomPanelHeightToLeaveTopPanelLines(int lineCount)
        {
            return 1600 - (lineCount * 100);
        } 
    }
}