using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
                // Intermediate Key-Value pair
                var kvPairs = new List<KeyValuePair<string, string>>();
                // Query providers
                kvPairs.AddRange(content.m_queryProviders.SelectMany(k => k.ToKeyValuePairCollection()));
                // Base parameters
                kvPairs.AddRange(content.ToKeyValuePairCollection());

                // Set Raw continuation to true
                kvPairs.Add(new KeyValuePair<string, string>("rawcontinue", "1"));

                // Add format config
                kvPairs.AddRange(JsonFormatConfig.FormatConfig);

                return new FormUrlEncodedContent(kvPairs);
            }, ctkn);
        }
    }
}
