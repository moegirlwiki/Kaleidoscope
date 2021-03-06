//-----------------------------------------------------------------------
// <copyright file="QueryResponseDeserializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform.Internals;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Actions.Transform
{
    public class QueryResponseDeserializer : IResponseDeserializer<QueryResponse>
    {
        private readonly QueryInputModel m_inputModel;
        private const string RawContinueKey = "query-continue";

        public QueryResponseDeserializer(QueryInputModel inputModel)
        {
            m_inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }

        public async Task<QueryResponse> DeserializeResponseAsync(HttpContent input, CancellationToken ctkn)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (input)
            {
                if (input.Headers.ContentType?.MediaType != JsonFormatConfig.MimeType)
                    throw new InvalidOperationException("MIME type mismatch");

                var content = await input.ReadAsStringAsync();
                var jsonEntity = await Task.Factory.StartNew(() => GateKeeper.ParseJObject(content), ctkn);

                // Get Key
                var rAttrib = typeof(QueryResponse).GetTypeInfo().GetCustomAttribute<ApiResponseAttribute>();
                var querySubKey = rAttrib?.Name;

                if (querySubKey == null) throw new InvalidOperationException("No key defined");

                var queryEntity = jsonEntity[querySubKey];
                if (queryEntity == null) throw new KeyNotFoundException(GateKeeper.PayloadNotFound);

                // Get continuation token
                var continuationTokens = queryEntity[RawContinueKey];

                // Pass to downstream
                return new QueryResponse(m_inputModel, continuationTokens, queryEntity);
            }
        }
    }
}
