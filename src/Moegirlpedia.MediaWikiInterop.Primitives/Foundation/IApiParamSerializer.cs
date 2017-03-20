using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IAggregatedApiParamSerializer : IFinalizedApiParamSerializer
    {
        List<KeyValuePair<string, string>> SerializeAllFields(object entity);
    }

    public interface IFinalizedApiParamSerializer
    {
        string SerializeFinalizedFields(object entity);
    }
}
