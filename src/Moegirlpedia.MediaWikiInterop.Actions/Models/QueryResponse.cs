//-----------------------------------------------------------------------
// <copyright file="QueryResponse.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Query;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    [ApiResponse("query")]
    public class QueryResponse : IApiActionResponse
    {
        private readonly JToken m_continuationTokens;
        private readonly JToken m_queryResponses;
        private readonly QueryInputModel m_inputModel;

        private bool m_responseParsed;

        // Type refers to specific response type.
        // IQueryProvider - QueryProviderAttribute, Continuation Token, Object
        private readonly Dictionary<IQueryProvider, (QueryProviderAttribute, string, object)> m_parsedEntities;
        private readonly Dictionary<Type, (string, object)> m_typedParsedEntities;

        public QueryResponse(QueryInputModel queryInputModel, JToken continuationToken, JToken queryResponses)
        {
            m_continuationTokens = continuationToken;
            m_parsedEntities = new Dictionary<IQueryProvider, (QueryProviderAttribute, string, object)>();
            m_typedParsedEntities = new Dictionary<Type, (string, object)>();
            m_responseParsed = false;
            m_inputModel = queryInputModel ?? throw new ArgumentNullException(nameof(queryInputModel));
            m_queryResponses = queryResponses ?? throw new ArgumentNullException(nameof(queryResponses));
        }

        private void ParseQueryEntities()
        {
            if (m_responseParsed) return;

            // Determined by Input Model: Query query-continue. 
            // And parse content.
            foreach (var queryProvider in m_inputModel.m_queryProviders)
            {
                // Sanity checked: Just get attribute
                var qAttrib = queryProvider.GetType().GetTypeInfo().GetCustomAttribute<QueryProviderAttribute>();
                var contToken = m_continuationTokens?[qAttrib.ContinuationTokenId]?.ToString();
                var entityRaw = m_queryResponses[qAttrib.Name]?.ToString();
                var entity = JsonConvert.DeserializeObject(entityRaw, qAttrib.ResponseType);

                m_parsedEntities.Add(queryProvider, (qAttrib, contToken, entity));
                m_typedParsedEntities.Add(qAttrib.ResponseType, (contToken, entity));
            }

            m_responseParsed = true;
        }

        public QueryProviderResponse<TResponse> GetQueryProviderResponse<TResponse>(IQueryProvider<TResponse> provider)
        {
            if (!m_responseParsed) ParseQueryEntities();

            return m_parsedEntities.TryGetValue(provider, out (QueryProviderAttribute, string, object) pValue) ?
                new QueryProviderResponse<TResponse>((TResponse)pValue.Item3, pValue.Item2) :
                null;
        }

        public QueryProviderResponse<TResponse> GetQueryTypedResponse<TResponse>()
        {
            if (!m_responseParsed) ParseQueryEntities();

            return m_typedParsedEntities.TryGetValue(typeof(TResponse), out (string, object) pValue) ?
                new QueryProviderResponse<TResponse>((TResponse) pValue.Item2, pValue.Item1) :
                null;
        }
    }
}
