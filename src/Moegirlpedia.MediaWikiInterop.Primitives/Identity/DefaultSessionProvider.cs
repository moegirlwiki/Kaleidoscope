using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public class DefaultSessionProvider : ISessionProvider
    {
        public bool IsHeaderInjectionEnabled => false;

        public bool IsCookieInjectionEnabled => false;

        public Task<IEnumerable<KeyValuePair<string, string>>> GetHeaderAsync(Guid sessionIdentifier, 
            CancellationToken ctkn = default(CancellationToken))
        {
            throw new NotSupportedException();
        }

        public Task SetCookieAsync(Guid sessionIdentifier, CookieContainer container, 
            CancellationToken ctkn = default(CancellationToken))
        {
            throw new NotSupportedException();
        }
    }
}
