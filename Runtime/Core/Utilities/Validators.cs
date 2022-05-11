using System.Text.RegularExpressions;

namespace MoralisUnity.Sdk.Utilities
{
    /// <summary>
    /// Provides runtime validators
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Imperfect check to validate a Web3Address format
        /// From https://www.oodlestechnologies.com/blogs/validating-ethereum-address/
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValidWeb3AddressFormat(string address)
        {
	        Regex regex = new Regex("^0x[0-9a-f]{40}$");
	        return regex.Match(address).Success;
        }
    }
}