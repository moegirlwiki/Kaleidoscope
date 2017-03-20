using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IDictionarySerializable
    {
        Dictionary<string, string> Serialize();
    }
}
