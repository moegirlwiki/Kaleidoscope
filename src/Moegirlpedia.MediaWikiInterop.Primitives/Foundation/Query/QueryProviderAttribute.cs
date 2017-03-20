using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query
{
    public class QueryProviderAttribute : Attribute
    {
        public string Name { get; }
        public string ContinuationTokenId { get; }
        public string Category { get; }
        public Type ResponseType { get; }

        public QueryProviderAttribute(string name, string category, 
            Type responseType, string continuationTokenId = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            ResponseType = responseType ?? throw new ArgumentNullException(nameof(responseType));
            ContinuationTokenId = continuationTokenId;
        }
    }
}
