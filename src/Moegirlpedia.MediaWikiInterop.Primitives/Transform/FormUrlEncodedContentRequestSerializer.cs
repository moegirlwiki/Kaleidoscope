﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class FormUrlEncodedContentRequestSerializer<T> : IRequestSerializer<T> where T : IApiActionRequest
    {
        private static ConditionalWeakTable<Type, PropertyInfo[]> m_propMetadataCache 
            = new ConditionalWeakTable<Type, PropertyInfo[]>();

        private static readonly IReadOnlyList<KeyValuePair<string, string>> m_formatConfig = 
            new List<KeyValuePair<string, string>>
            {
                // Hard-code format information
                new KeyValuePair<string, string>("format", "json"),
                new KeyValuePair<string, string>("utf8", "1"),
                new KeyValuePair<string, string>("formatversion", "2")
            };

        public Task<HttpContent> SerializeRequestAsync(T content, CancellationToken ctkn = default(CancellationToken))
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return Task.Factory.StartNew<HttpContent>(() =>
            {
                // Intermediate type
                var cType = typeof(T);

                // Check if we have property information stored
                PropertyInfo[] properties = null;

                // Intermediate Key-Value pair
                var kvPairs = new List<KeyValuePair<string, string>>();
                // Add format config
                kvPairs.AddRange(m_formatConfig);

                // Query all properties with attribute
                if (!m_propMetadataCache.TryGetValue(cType, out properties))
                {
                    var propertiesQuery = cType.GetProperties().Where(p => p.IsDefined(typeof(ApiParameterAttribute)));
                    properties = propertiesQuery.ToArray();
                    m_propMetadataCache.Add(cType, properties);
                }

                // Call path is determined, so it is not necessary to check nullref here

                // Serialize properties
                foreach (var property in properties)
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
