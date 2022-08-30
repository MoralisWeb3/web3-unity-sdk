using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Net;
using MoralisUnity.SolanaApi.Client;
using MoralisUnity.SolanaApi.Interfaces;
using MoralisUnity.SolanaApi.Models;

namespace MoralisUnity.SolanaApi.CloudApi
{
    public class TokenApi : ITokenApi;
    
    {

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenApi"/> class.
		/// </summary>
		/// <param name="apiClient"> an instance of ApiClient (optional)</param>
		/// <returns></returns>
		public TokenApi(ApiClient apiClient = null)
		{
			if (apiClient == null) // use the default one in Configuration
				this.ApiClient = Configuration.DefaultApiClient;
			else
				this.ApiClient = apiClient;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NativeApi"/> class.
		/// </summary>
		/// <returns></returns>
		public TokenApi(String basePath)
		{
			this.ApiClient = new ApiClient(basePath);
		}

		/// <summary>
		/// Sets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public void SetBasePath(String basePath)
		{
			this.ApiClient.BasePath = basePath;
		}

		/// <summary>
		/// Gets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public String GetBasePath(String basePath)
		{
			return this.ApiClient.BasePath;
		}

		/// <summary>
		/// Gets or sets the API client.
		/// </summary>
		/// <value>An instance of the ApiClient</value>
		public ApiClient ApiClient { get; set; }
		
		
		public async UniTask<TokenPrice> GetTokenPrice(NetworkTypes network, string address) {
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenPrice");
			
			var headerParams = new Dictionary<String, String>();

			var path = "/token/{network}/{address}/price";
			path = path.Replace("{" + "network" + "}", ApiClient.ParameterToString(network.ToString()));
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			Tuple<HttpStatusCode, Dictionary<string, string>, string> response = await ApiClient.CallApi(path, Method.GET, null, null, headerParams, null, null, authSettings);
			
			if (((int)response.Item1) >= 400)
				throw new ApiException((int)response.Item1, "Error calling GetTokenPrice: " + response.Item3, response.Item3);
			else if (((int)response.Item1) == 0)
				throw new ApiException((int)response.Item1, "Error calling GetTokenPrice: " + response.Item3, response.Item3);

			
			return ((CloudFunctionResult<TokenPrice>)ApiClient.Deserialize(response.Item3, typeof(CloudFunctionResult<TokenPrice>), response.Item2)).Result;

			
			}

	}    

}