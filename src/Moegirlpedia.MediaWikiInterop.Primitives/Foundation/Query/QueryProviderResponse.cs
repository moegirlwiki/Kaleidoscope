using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query
{
    public class QueryProviderResponse<TResponse>
    {
        TResponse Response { get; }
        string ContinuationToken { get; }

        internal QueryProviderResponse(TResponse response, string continuationToken)
        {
            Response = response;
            ContinuationToken = continuationToken;
        }
    }
}
