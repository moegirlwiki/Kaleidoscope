using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query
{
    public interface IQueryProvider
    {
        List<KeyValuePair<string, string>> GetParameters();
    }

    public interface IQueryProvider<TResponse> : IQueryProvider
    {

    }
}
