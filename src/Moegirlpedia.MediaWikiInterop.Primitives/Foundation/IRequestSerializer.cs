using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IRequestSerializer<TRequest>
    {
        Task<HttpContent> SerializeRequestAsync();
    }
}
