/**
 *           Module: ChainEntry.cs
 *  Descriptiontion: Provides a easy way to get detail about an EVM chain for 
 *  all EVM chains supported by the Moralis Web3API
 *           Author: Moralis Web3 Technology AB, 559307-5988 - David B. Goodrich 
 *  
 *  MIT License
 *  
 *  Copyright (c) 2021 Moralis Web3 Technology AB, 559307-5988
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using MoralisUnity.Web3Api.Models;

namespace MoralisUnity
{
    /// <summary>
    /// Provides a easy way to get detail about an EVM chain for all EVM chains 
    /// supported by the Moralis Web3API
    /// </summary>
    public class SupportedEvmChains
    {
        private static List<ChainEntry> chains = new List<ChainEntry>();

        /// <summary>
        /// The list of EVM chains supported by the Moralis Web3API.
        /// </summary>
        public static List<ChainEntry> SupportedChains 
        { 
            get 
            {
                if (chains.Count < 1) PopulateChainList();

                return chains; 
            } 
        }

        /// <summary>
        /// Retrieve an chain entry by enum value.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ChainEntry FromChainList(ChainList target)
        {
            ChainEntry result = null;

            var searchResult = from c in SupportedChains
                               where target.Equals(c.EnumValue)
                               select c;

            result = searchResult.FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Retrieve an chain entry by enum value.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ChainEntry FromChainList(string target)
        {
            ChainEntry result = null;

            var searchResult = from c in SupportedChains
                               where target.Equals(c.Name)
                               select c;

            result = searchResult.FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Retrieve an chain entry by enum value.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ChainEntry FromChainList(int target)
        {
            ChainEntry result = null;

            var searchResult = from c in SupportedChains
                               where target.Equals(c.ChainId)
                               select c;

            result = searchResult.FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Loops through the current ChainList enum and builds a friendly to use 
        /// name / chainId, enum val entry.
        /// </summary>
        private static void PopulateChainList()
        {
            Dictionary<string, Dictionary<string, object>> chainExtras = GetChainDetails();
            chains.Clear();

            foreach (ChainList chain in Enum.GetValues(typeof(ChainList)))
            {
                ChainEntry ce = new ChainEntry()
                {
                    ChainId = (int)chain,
                    EnumValue = chain,
                    Name = chain.ToString()
                };

                if (chainExtras.ContainsKey(ce.Name))
                {
                    ce.Symbol = chainExtras[ce.Name]["symbol"].ToString();
                    ce.Decimals = int.Parse(chainExtras[ce.Name]["decimals"].ToString());
                }

                chains.Add(ce);
            }
        }

        /// <summary>
        /// Define extra information about the EVM chanis supported by Moralis.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, object>> GetChainDetails()
        {
            Dictionary<string, Dictionary<string, object>> resp = new Dictionary<string, Dictionary<string, object>>();

            resp.Add("eth", new Dictionary<string, object>());
            resp["eth"].Add("symbol", "ETH");
            resp["eth"].Add("decimals", 18);

            resp.Add("ropsten", new Dictionary<string, object>());
            resp["ropsten"].Add("symbol", "ETH");
            resp["ropsten"].Add("decimals", 18);

            resp.Add("rinkeby", new Dictionary<string, object>());
            resp["rinkeby"].Add("symbol", "ETH");
            resp["rinkeby"].Add("decimals", 18);

            resp.Add("goerli", new Dictionary<string, object>());
            resp["goerli"].Add("symbol", "ETH");
            resp["goerli"].Add("decimals", 18);

            resp.Add("kovan", new Dictionary<string, object>());
            resp["kovan"].Add("symbol", "ETH");
            resp["kovan"].Add("decimals", 18);

            resp.Add("polygon", new Dictionary<string, object>());
            resp["polygon"].Add("symbol", "MATIC");
            resp["polygon"].Add("decimals", 18);

            resp.Add("mumbai", new Dictionary<string, object>());
            resp["mumbai"].Add("symbol", "MATIC");
            resp["mumbai"].Add("decimals", 18);

            resp.Add("bsc", new Dictionary<string, object>());
            resp["bsc"].Add("symbol", "BNB");
            resp["bsc"].Add("decimals", 18);

            resp.Add("bsc_testnet", new Dictionary<string, object>());
            resp["bsc_testnet"].Add("symbol", "BNB");
            resp["bsc_testnet"].Add("decimals", 18);

            resp.Add("avalanche", new Dictionary<string, object>());
            resp["avalanche"].Add("symbol", "AVAX");
            resp["avalanche"].Add("decimals", 18);

            resp.Add("avalanche_testnet", new Dictionary<string, object>());
            resp["avalanche_testnet"].Add("symbol", "AVAX");
            resp["avalanche_testnet"].Add("decimals", 18);

            resp.Add("fantom", new Dictionary<string, object>());
            resp["fantom"].Add("symbol", "FTM");
            resp["fantom"].Add("decimals", 18);

            resp.Add("cronos", new Dictionary<string, object>());
            resp["cronos"].Add("symbol", "CRO");
            resp["cronos"].Add("decimals", 18);

            resp.Add("cronos_testnet", new Dictionary<string, object>());
            resp["cronos_testnet"].Add("symbol", "CRO");
            resp["cronos_testnet"].Add("decimals", 18);
            
            return resp;
        }
    }
}
