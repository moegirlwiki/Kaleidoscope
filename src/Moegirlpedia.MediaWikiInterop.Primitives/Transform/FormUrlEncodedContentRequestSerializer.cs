using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class FormUrlEncodedContentRequestSerializer<T> : IRequestSerializer<T> where T : IApiActionRequest
    {
        public Task<HttpContent> SerializeRequestAsync(T content, CancellationToken ctkn = default(CancellationToken))
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return Task.Factory.StartNew<HttpContent>(() =>
            {
                // Intermediate type
                var cType = typeof(T);

                // Intermediate Key-Value pair
                var kvPairs = new List<KeyValuePair<string, string>>();

                // Hard-code format information
                kvPairs.Add(new KeyValuePair<string, string>("format", "json"));
                kvPairs.Add(new KeyValuePair<string, string>("utf8", "1"));
                kvPairs.Add(new KeyValuePair<string, string>("formatversion", "2"));

                // Query all properties with attribute
                var propertiesQuery = cType.GetProperties().Where(p => p.IsDefined(typeof(ApiParameterAttribute)));

                // Serialize properties
                foreach (var property in propertiesQuery)
                {
                    // Null check
                    var pValue = property.GetValue(content);
                    if (pValue == null) continue;

                    // Attribute
                    var pAttrib = property.GetCustomAttribute<ApiParameterAttribute>();

                    // Aggreated property
                    if (pAttrib.IsAggregated)
                    {
                        var serializer = (IAggregatedApiParamSerializer)Activator.CreateInstance(pAttrib.CustomConverter);
                        kvPairs.AddRange(serializer.SerializeAllFields(pValue));
                        continue;
                    }

                    // Finalized property
                    string serValue = null;
                    switch (pValue)
                    {
                        case bool bValue:
                            serValue = bValue ? "1" : "0";
                            break;
                        case string sValue:
                            serValue = sValue;
                            break;
                        case int iValue:
                            serValue = iValue.ToString("D");
                            break;
                        case List<int> liValue:
                            serValue = string.Join("|", liValue.Select(i => i.ToString("D")));
                            break;
                        case List<string> lsValue:
                            serValue = string.Join("|", lsValue);
                            break;
                    }

                    kvPairs.Add(new KeyValuePair<string, string>(pAttrib.Name, serValue));
                }

                return new FormUrlEncodedContent(kvPairs);
            }, ctkn);
        }
    }
}
