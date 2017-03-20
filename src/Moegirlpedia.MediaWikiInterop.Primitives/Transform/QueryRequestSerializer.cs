using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class QueryRequestSerializer : IRequestSerializer<QueryInputModel>
    {
        public Task<HttpContent> SerializeRequestAsync(QueryInputModel content, 
            CancellationToken ctkn = default(CancellationToken))
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return Task.Factory.StartNew<HttpContent>(() =>
            {
                return new FormUrlEncodedContent(GetKeyPairs(content));
            }, ctkn);
        }

        internal List<KeyValuePair<string, string>> GetKeyPairs(QueryInputModel content)
        {
            // Intermediate Key-Value pair
            var kvPairs = new List<KeyValuePair<string, string>>();

            // Query providers
            kvPairs.AddRange(content.m_queryProviders.SelectMany(k => k.GetParameters()));

            // Query providers meta
            kvPairs.AddRange(content.m_queryProviders.Select(i => i.GetType().GetTypeInfo().GetCustomAttribute<QueryProviderAttribute>())
                .GroupBy(i => i.Category)
                .Select(i => new KeyValuePair<string, string>(i.Key, string.Join("|", i.Select(a => a.Name)))));

            // Base parameters
            kvPairs.AddRange(content.ToKeyValuePairCollection());

            // Set Raw continuation to true
            kvPairs.Add(new KeyValuePair<string, string>("rawcontinue", "1"));

            // Add format config
            kvPairs.AddRange(JsonFormatConfig.FormatConfig);

            return kvPairs;
        }
    }
}
