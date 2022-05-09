
namespace MoralisUnity.Sdk.Constants
{
    /// <summary>
    /// Helper values
    /// </summary>
    public static class MoralisConstants 
    {
		//Paths
        public const string PathMoralisCreateAssetMenu = Moralis + "/" + Web3UnitySDK;
        public const string PathMoralisWindowMenu = "Window/" + Moralis + "/" + Web3UnitySDK;
        public const string PathMoralisExamplesCreateAssetMenu = PathMoralisCreateAssetMenu + "/Examples";
        public const string PathMoralisExamplesWindowMenu = PathMoralisWindowMenu + "/Examples";
        public const string PathMoralisSamplesCreateAssetMenu = PathMoralisCreateAssetMenu + "/Samples";
        public const string PathMoralisSamplesWindowMenu = PathMoralisWindowMenu + "/Samples";
        
        // Skipping ">10" shows a horizontal divider line.
        public const int PriorityMoralisWindow_Primary = 10;
        public const int PriorityMoralisWindow_Secondary = 100;
        public const int PriorityMoralisWindow_Examples = 1000;
        public const int PriorityMoralisWindow_Samples = 10000;

		//
		public const string Moralis = "Moralis";
		public const string Web3UnitySDK = "Web3 Unity SDK";
		public const string Open = "Open";
		public const string OpenReadMe = Open + " " + "ReadMe";

    }
}