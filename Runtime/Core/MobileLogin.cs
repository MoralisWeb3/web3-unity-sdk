/**
 *           Module: MobileLogin.cs
 *  Descriptiontion: Class that enables use of Moralis Connector for suthentication.
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
using Newtonsoft.Json;
using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;
using MoralisUnity;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity
{
    public class MobileLogin
    {
        private static string TOKEN_REQUEST_URL = "server/requestLoginToken";
        private static string REMOTE_SESSION_URL = "server/getRemoteSession?login_token={0}&_ApplicationId={1}";

        public static async UniTask<MoralisUser> LogIn(string moralisDappUrl, string dappId)
        {
            MoralisUser user = null;
            MoralisLoginTokenResponse tokenResponse = await RequestLoginToken(moralisDappUrl, dappId);

            if (tokenResponse != null)
            {
                // Display the connector page.
                Application.OpenURL(tokenResponse.url);

                DateTime timeout = DateTime.Now.AddSeconds(120);

                while (true && DateTime.Now < timeout)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(500), ignoreTimeScale: false);

                    MoralisSessionTokenResponse sessionResponse = await CheckSessionResult(moralisDappUrl, tokenResponse.loginToken, dappId);

                    if (sessionResponse != null && !String.IsNullOrWhiteSpace(sessionResponse.sessionToken))
                    {
                        user = await Moralis.GetClient().UserFromSession(sessionResponse.sessionToken);

                        break;
                    }
                }
            }

            return user;
        }

        private async static UniTask<MoralisSessionTokenResponse> CheckSessionResult(string moralisDappUrl, string tokenId, string dappId)
        {
            MoralisSessionTokenResponse result = null;

            MoralisLoginTokenRequest payload = new MoralisLoginTokenRequest()
            {
                _ApplicationId = dappId
            };

            string data = JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(moralisDappUrl);

                HttpResponseMessage response = client.GetAsync(String.Format(REMOTE_SESSION_URL, tokenId, dappId)).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Debug.Log($"Session Result: {responseBody}");

                    result = JsonConvert.DeserializeObject<MoralisSessionTokenResponse>(responseBody);
                }
            }

            return result;
        }

        private async static UniTask<MoralisLoginTokenResponse> RequestLoginToken(string moralisDappUrl, string dappId)
        {
            MoralisLoginTokenResponse result = null;

            MoralisLoginTokenRequest payload = new MoralisLoginTokenRequest()
            {
                _ApplicationId = dappId
            };

            string data = JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(moralisDappUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(data);
                // List data response.
                HttpResponseMessage response = client.PostAsync(TOKEN_REQUEST_URL, content).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    string responseBody = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<MoralisLoginTokenResponse>(responseBody);
                }
            }
            
            return result;
        }
    }
}
