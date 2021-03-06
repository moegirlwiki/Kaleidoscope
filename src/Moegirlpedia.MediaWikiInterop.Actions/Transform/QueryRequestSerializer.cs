//-----------------------------------------------------------------------
// <copyright file="QueryRequestSerializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Actions.Query;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Actions.Transform
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
