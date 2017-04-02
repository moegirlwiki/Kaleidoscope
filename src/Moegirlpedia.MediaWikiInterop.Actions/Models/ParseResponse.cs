using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    [ApiResponse("parse")]
    public class ParseResponse : IApiActionResponse
    {
        private JToken m_rawObject = null;

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("pageid")]
        public int PageId { get; set; }

        internal void SetRawData(JToken token)
        {
            m_rawObject = token;
        }

        public T GetPrimitiveTypedField<T>(ParseProp prop)
        {
            var entity = m_rawObject[prop.ToString().ToLower()];
            return (entity == null) ? default(T) : entity.Value<T>();
        }

        public T GetReferenceTypedField<T>()
        {
            // Get defined attribute
            var type = typeof(T).GetTypeInfo();
            if (!type.IsDefined(typeof(ParseResponseSubEntityAttribute))) return default(T);

            var attrib = type.GetCustomAttribute<ParseResponseSubEntityAttribute>();
            var entity = m_rawObject[attrib.Type.ToString().ToLower()];
            return (entity == null) ? default(T) : JsonConvert.DeserializeObject<T>(entity.Value<string>());
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParseResponseSubEntityAttribute : Attribute
    {
        public ParseProp Type { get; }

        public ParseResponseSubEntityAttribute(ParseProp type)
        {
            Type = type;
        }
    }
}
