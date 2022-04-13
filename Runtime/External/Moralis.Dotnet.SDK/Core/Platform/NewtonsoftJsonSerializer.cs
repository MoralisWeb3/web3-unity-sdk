using Moralis.Platform.Abstractions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;
using System.Globalization;

namespace Moralis.Platform
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public IDictionary<string, object> DefaultOptions { get; set; }

        public NewtonsoftJsonSerializer()
        {
            DefaultOptions = new Dictionary<string, object>();
            DefaultOptions.Add("NullValueHandling", NullValueHandling.Ignore);
            DefaultOptions.Add("ReferenceLoopHandling", ReferenceLoopHandling.Serialize);
            DefaultOptions.Add("DateFormatString", "yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public T Deserialize<T>(string json, IDictionary<string, object> options=null)
        {
            if (options != null)
            {
                return JsonConvert.DeserializeObject<T>(json, OptionsToSettings(options));
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public string Serialize(object target, IDictionary<string, object> options=null)
        {
            if (options != null)
            {
                return JsonConvert.SerializeObject(target, OptionsToSettings(options));
            }
            else
            {
                return JsonConvert.SerializeObject(target);
            }
        }

        private JsonSerializerSettings OptionsToSettings(IDictionary<string, object> options)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                //CheckAdditionalContent = options.ContainsKey("CheckAdditionalContent") ? (bool)options["CheckAdditionalContent"] : false,
                //ConstructorHandling = options.ContainsKey("ConstructorHandling") ? (ConstructorHandling)options["ConstructorHandling"] : ConstructorHandling.Default,
                //Context = options.ContainsKey("Context") ? (StreamingContext)options["Context"] : new StreamingContext(),
                //ContractResolver = options.ContainsKey("ContractResolver") ? (IContractResolver)options["ContractResolver"] : null,
                //Converters = options.ContainsKey("Converters") ? (List<JsonConverter>)options["Converters"] : null,
                //Culture = options.ContainsKey("Culture") ? (CultureInfo)options["Culture"] : null,
                //DateFormatHandling = options.ContainsKey("DateFormatHandling") ? (DateFormatHandling)options["DateFormatHandling"] : DateFormatHandling.IsoDateFormat,
                DateFormatString = options.ContainsKey("DateFormatString") ? (string)options["DateFormatString"] : null,
                //DateParseHandling = options.ContainsKey("DateParseHandling") ? (DateParseHandling)options["DateParseHandling"] : DateParseHandling.DateTime,
                //DateTimeZoneHandling = options.ContainsKey("DateTimeZoneHandling") ? (DateTimeZoneHandling)options["DateTimeZoneHandling"] : DateTimeZoneHandling.Utc,
                //DefaultValueHandling = options.ContainsKey("DefaultValueHandling") ? (DefaultValueHandling)options["DefaultValueHandling"] : DefaultValueHandling.Ignore,
                //Error = options.ContainsKey("Error") ? (EventHandler<ErrorEventArgs>)options["Error"] : null,
                //FloatFormatHandling = options.ContainsKey("FloatFormatHandling") ? (FloatFormatHandling)options["FloatFormatHandling"] : FloatFormatHandling.DefaultValue,
                //FloatParseHandling = options.ContainsKey("FloatParseHandling") ? (FloatParseHandling)options["FloatParseHandling"] : FloatParseHandling.Double,
                //Formatting = options.ContainsKey("Formatting") ? (Formatting)options["Formatting"] : Formatting.None,
                //MaxDepth = options.ContainsKey("MaxDepth") ? (int?)options["MaxDepth"] : null,
                //MetadataPropertyHandling = options.ContainsKey("MetadataPropertyHandling") ? (MetadataPropertyHandling)options["MetadataPropertyHandling"] : MetadataPropertyHandling.Default,
                //MissingMemberHandling = options.ContainsKey("MissingMemberHandling") ? (MissingMemberHandling)options["MissingMemberHandling"] : MissingMemberHandling.Ignore,
                NullValueHandling = options.ContainsKey("NullValueHandling") ? (NullValueHandling)options["NullValueHandling"] : NullValueHandling.Ignore,
                //ObjectCreationHandling = options.ContainsKey("ObjectCreationHandling") ? (ObjectCreationHandling)options["ObjectCreationHandling"] : ObjectCreationHandling.Auto,
                //PreserveReferencesHandling = options.ContainsKey("PreserveReferencesHandling") ? (PreserveReferencesHandling)options["PreserveReferencesHandling"] : PreserveReferencesHandling.None,
                ReferenceLoopHandling = options.ContainsKey("ReferenceLoopHandling") ? (ReferenceLoopHandling)options["ReferenceLoopHandling"] : ReferenceLoopHandling.Ignore,
                //ReferenceResolverProvider = options.ContainsKey("ReferenceResolverProvider") ? (Func<IReferenceResolver>)options["ReferenceResolverProvider"] : null,
                //SerializationBinder = options.ContainsKey("SerializationBinder") ? (ISerializationBinder)options["SerializationBinder"] : null,
                //StringEscapeHandling = options.ContainsKey("StringEscapeHandling") ? (StringEscapeHandling)options["StringEscapeHandling"] : StringEscapeHandling.Default,
                //TraceWriter = options.ContainsKey("TraceWriter") ? (ITraceWriter)options["TraceWriter"] : null,
                //TypeNameAssemblyFormatHandling = options.ContainsKey("TypeNameAssemblyFormatHandling") ? (TypeNameAssemblyFormatHandling)options["TypeNameAssemblyFormatHandling"] : TypeNameAssemblyFormatHandling.Full,
                //TypeNameHandling = options.ContainsKey("TypeNameHandling") ? (TypeNameHandling)options["TypeNameHandling"] : TypeNameHandling.None
            };

            return settings;
        }
    }
}
