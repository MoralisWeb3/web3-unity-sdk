
namespace MoralisUnity.Sdk.Utilities
{
    /// <summary>
    /// Provides runtime formatters
    /// </summary>
    public static class Formatters
    {
        /// <summary>
        /// Returns a string of form "abc...xyz"
        /// <see cref="https://github.com/web3ui/web3uikit/blob/master/src/web3utils/formatters.ts"/>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetWeb3AddressShortFormat(string str, int n = 6)
        {
	        if (string.IsNullOrEmpty(str))
	        {
		        return string.Empty;
	        }
	        
	        if (str.Length < n)
	        {
		        return str;
	        }

	        return $"{str.Substring(0, n)}...{str.Substring(str.Length - n)}";
        }
    }
}