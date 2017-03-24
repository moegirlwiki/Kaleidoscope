//-----------------------------------------------------------------------
// <copyright file="QueryProviderResponse.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moegirlpedia.MediaWikiInterop.Actions.Query
{
    public class QueryProviderResponse<TResponse>
    {
        public TResponse Response { get; }
        public string ContinuationToken { get; }

        public QueryProviderResponse(TResponse response, string continuationToken)
        {
            Response = response;
            ContinuationToken = continuationToken;
        }
    }
}
