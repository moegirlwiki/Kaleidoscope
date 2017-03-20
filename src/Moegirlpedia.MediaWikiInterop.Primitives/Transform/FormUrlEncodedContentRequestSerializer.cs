//-----------------------------------------------------------------------
// <copyright file="FormUrlEncodedContentRequestSerializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class FormUrlEncodedContentRequestSerializer<T> : IRequestSerializer<T> where T : IApiActionRequest
    {
        public Task<HttpContent> SerializeRequestAsync(T content, CancellationToken ctkn = default(CancellationToken))
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return Task.Factory.StartNew<HttpContent>(() =>
            {
                // Intermediate Key-Value pair
                var kvPairs = new List<KeyValuePair<string, string>>();
                // Add format config
                kvPairs.AddRange(JsonFormatConfig.FormatConfig);
                // Serialize content
                kvPairs.AddRange(content.ToKeyValuePairCollection());

                return new FormUrlEncodedContent(kvPairs);
            }, ctkn);
        }
    }
}
