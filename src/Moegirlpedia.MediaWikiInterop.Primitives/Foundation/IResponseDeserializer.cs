using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IResponseDeserializer<TResponse>
    {
        Task<TResponse> DeserializeResponseAsync(string input);
    }
}
