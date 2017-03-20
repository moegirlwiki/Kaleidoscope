using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action.Models
{
    [ApiResponse("query")]
    public class QueryResponse : IApiActionResponse
    {
        private readonly JObject m_continuationTokens;
        private readonly JObject m_queryResponses;
        private readonly QueryInputModel m_inputModel;

        private bool m_continuationTokensParsed;
        private bool m_responseParsed;

        // Type refers to specific response type.
        // IQueryProvider - QueryProviderAttribute, Continuation Token, Object
        private readonly Dictionary<IQueryProvider, (QueryProviderAttribute, string, object)> m_parsedEntities;

        public QueryResponse(QueryInputModel queryInputModel, JObject continuationToken, JObject queryResponses)
        {
            m_continuationTokens = continuationToken;
            m_parsedEntities = new Dictionary<IQueryProvider, (QueryProviderAttribute, string, object)>();
            m_continuationTokensParsed = false;
            m_responseParsed = false;
            m_inputModel = queryInputModel ?? throw new ArgumentNullException(nameof(queryInputModel));
            m_queryResponses = queryResponses ?? throw new ArgumentNullException(nameof(queryResponses));
        }

        private void ParseContinuationTokens()
        {
            // Check if we have continuation token
            if (m_continuationTokens == null) return;

            // Determined by Input Model: Query query-continue.
            foreach (var queryProvider in m_inputModel.m_queryProviders)
            {
                // Sanity checked: Just get attribute
                var qAttrib = queryProvider.GetType().GetTypeInfo().GetCustomAttribute<QueryProviderAttribute>();

                var contToken = m_continuationTokens[qAttrib.ContinuationTokenId]?.ToString();
                m_parsedEntities.Add(queryProvider, (qAttrib, contToken, null));
            }

            // Set flag
            m_continuationTokensParsed = true;
        }

        private void ParseQueryEntities()
        {
            if (!m_continuationTokensParsed) ParseContinuationTokens();

            foreach (var queryProvider in m_inputModel.m_queryProviders)
            {
                var qTuple = m_parsedEntities[queryProvider];
                var entityRaw = m_queryResponses[qTuple.Item1.Name]?.ToString();
                if (entityRaw != null) qTuple.Item3 = JsonConvert.DeserializeObject(entityRaw, qTuple.Item1.ResponseType);
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
    }
}
