using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using MoralisUnity.Web3Api.Models;

namespace MoralisUnity.Web3Api.Interfaces
{
	/// <summary>
	/// Represents a collection of functions to interact with the API endpoints
	/// </summary>
	public interface IStorageApi
	{
		/// <summary>
		/// Uploads multiple files and place them in a folder directory
		/// 
		/// </summary>
		/// <param name="abi">Array of JSON and Base64 Supported</param>
		/// <returns>Returns the path to the uploaded files</returns>
		UniTask<List<IpfsFile>> UploadFolder (List<IpfsFileRequest> abi);

	}
}
