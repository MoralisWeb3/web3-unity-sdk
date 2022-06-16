using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using MoralisUnity.Platform.Utilities;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Abstractions;
using Newtonsoft.Json;
using UnityEngine;
using MoralisUnity.Core.Exceptions;

namespace MoralisUnity.Platform.Objects
{

    class MoralisUserJsonConvertor : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null || !(value is MoralisUser))
            {
                serializer.Serialize(writer, null);
                return;
            }

            Dictionary<string, object> user = ((MoralisUser)value).ToParameterDictionary();

            serializer.Serialize(writer, user);
        }
    }

    [JsonConverter(typeof(MoralisUserJsonConvertor))]
    public class MoralisUser : MoralisObject
    {
        private bool isSaving = false;

        public MoralisUser()
        {
            this.ClassName = "_User";
        }

        public MoralisUser(string objectId = null,
            string userName = null,
            IDictionary<string, IDictionary<string, object>> authData = null,
            DateTime? createdAt = null,
            DateTime? updatedAt = null,
            MoralisAcl ACL = null,
            string sessionToken = null,
            string ethAddress = null
            ) : base("_User", objectId, sessionToken, createdAt, updatedAt, ACL)
        {
            this.username = userName;
            this.authData = authData != null ? authData : new Dictionary<string, IDictionary<string, object>>();

            if (!String.IsNullOrEmpty(ethAddress))
            {
                this.ethAddress = ethAddress;
                this.accounts = new string[1];
                this.accounts[0] = ethAddress;
            }
            else
            {
                accounts = new string[0];
            }
        }

        public string username;

        public IDictionary<string, IDictionary<string, object>> authData; 

        public string password;
        public string email;
        public string ethAddress;
        public string[] accounts;

        internal static IDictionary<string, IAuthenticationProvider> Authenticators { get; } = new Dictionary<string, IAuthenticationProvider> { };
    
        internal static HashSet<string> ImmutableKeys { get; } = new HashSet<string> { "classname", "sessionToken", "isNew" };
       
        internal ICurrentUserService<MoralisUser> CurrentUserService { get; set; }

        internal async UniTask SignUpAsync(UniTask toAwait, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(this.objectId))
            {
                if (String.IsNullOrEmpty(this.username)) throw new ArgumentException("User username required for this action.");
                if (String.IsNullOrEmpty(this.password)) throw new ArgumentException("User password required for this action.");

                await this.SaveAsync();
            }
        }

        public Dictionary<string, object> ToParameterDictionary()
        {
            List<string> propertiesToSkip = new List<string>(new string[] { "classname", "createdat", "sessiontoken" });
            Dictionary<string, object> result = new Dictionary<string, object>();

            // Use reflection to get all string properties 
            // that have getters and setters
            IEnumerable<PropertyInfo> properties = from p in this.GetType().GetProperties()
                                                   where p.CanRead &&
                                                         p.CanWrite
                                                   select p;

            foreach (PropertyInfo property in properties)
            {
                // Not all properties should be included on save
                if (isSaving && propertiesToSkip.Contains(property.Name.ToLower())) continue;

                var value = property.GetValue(this);

                if (value != null)
                {
                    result.Add(property.Name, value);
                }
            }

            IEnumerable<FieldInfo> fields = from f in this.GetType().GetFields()
                                            where f.IsPublic
                                            select f;

            foreach (FieldInfo field in fields)
            {
                // Not all properties should be included on save
                if (isSaving && propertiesToSkip.Contains(field.Name.ToLower())) continue;

                var value = field.GetValue(this);

                if (value != null)
                {
                    result.Add(field.Name, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Signs up a new user. This will create a new ParseUser on the server and will also persist the
        /// session on disk so that you can access the user using <see cref="CurrentUser"/>. A username and
        /// password must be set before calling SignUpAsync.
        /// </summary>
        public UniTask SignUpAsync() => SignUpAsync(CancellationToken.None);
        
        public void SetSaving(bool val)
        {
            isSaving = val;
        }

        /// <summary>
        /// Signs up a new user. This will create a new ParseUser on the server and will also persist the
        /// session on disk so that you can access the user using <see cref="CurrentUser"/>. A username and
        /// password must be set before calling SignUpAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async UniTask SignUpAsync(CancellationToken cancellationToken) //=> TaskQueue.Enqueue(toAwait => SignUpAsync(toAwait, cancellationToken), cancellationToken);
        {
            if (String.IsNullOrEmpty(this.objectId))
            {
                if (String.IsNullOrEmpty(this.username)) throw new ArgumentException("User username required for this action.");
                if (String.IsNullOrEmpty(this.password)) throw new ArgumentException("User password required for this action.");

                try
                {
                    await this.SaveAsync();
                }
                catch (MoralisSaveException msexp)
                {
                    throw new MoralisSignupException(msexp.Message);
                }
            }
        }

        /// <summary>
        /// Removes null values from authData (which exist temporarily for unlinking)
        /// </summary>
        void CleanupAuthData()
        {
            lock (Mutex)
            {
                IDictionary<string, IDictionary<string, object>> _authData = authData;

                if (_authData == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, IDictionary<string, object>> pair in new Dictionary<string, IDictionary<string, object>>(_authData))
                {
                    if (pair.Value == null)
                    {
                        _authData.Remove(pair.Key);
                    }
                }
            }
        }
        
        internal static IAuthenticationProvider GetProvider(string providerName) => Authenticators.TryGetValue(providerName, out IAuthenticationProvider provider) ? provider : null;

        /// <summary>
        /// Synchronizes authData for all providers.
        /// </summary>
        internal void SynchronizeAllAuthData()
        {
            lock (Mutex)
            {
                IDictionary<string, IDictionary<string, object>> _authData = authData;

                if (_authData == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, IDictionary<string, object>> pair in _authData)
                {
                    SynchronizeAuthData(GetProvider(pair.Key));
                }
            }
        }

        internal void SynchronizeAuthData(IAuthenticationProvider provider)
        {
            bool restorationSuccess = false;

            lock (Mutex)
            {
                IDictionary<string, IDictionary<string, object>> _authData = authData;

                if (_authData == null || provider == null)
                {
                    return;
                }

                if (_authData.TryGetValue(provider.AuthType, out IDictionary<string, object> data))
                {
                    restorationSuccess = provider.RestoreAuthentication(data);
                }
            }
        }

        /// <summary>
        /// Checks whether a user is linked to a service.
        /// </summary>
        internal bool IsLinked(string authType)
        {
            lock (Mutex)
            {
                return authData != null && authData.ContainsKey(authType) && authData[authType] != null;
            }
        }
    }
}
