using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IMultipleApiParamSerializer : IFinalizedApiParamSerializer
    {
        Dictionary<string, string> SerializeSubFields();
    }

    public interface IFinalizedApiParamSerializer
    {
        string SerializeFinalizedFields();
    }
}
