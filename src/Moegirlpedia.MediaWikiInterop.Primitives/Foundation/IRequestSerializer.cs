using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IRequestSerializer<TRequest> where TRequest : IApiActionRequest
    {
        Task<HttpContent> SerializeRequestAsync(TRequest content, CancellationToken ctkn = default(CancellationToken));
    }
}
