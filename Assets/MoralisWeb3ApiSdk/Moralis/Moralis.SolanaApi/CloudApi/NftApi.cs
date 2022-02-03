using Moralis.SolanaApi.Client;
using Moralis.SolanaApi.Interfaces;
using Moralis.SolanaApi.Models;
using System;
using System.Threading.Tasks;

namespace Moralis.SolanaApi.CloudApi
{
	public class NftApi : INftApi
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NativeApi"/> class.
		/// </summary>
		/// <param name="apiClient"> an instance of ApiClient (optional)</param>
		/// <returns></returns>
		public NftApi(ApiClient apiClient = null)
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
		public NftApi(String basePath)
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


		public Task<NftMetadata> GetNFTMetadata(NetworkTypes network, string address)
		{
			throw new NotImplementedException();
		}
	}
}
