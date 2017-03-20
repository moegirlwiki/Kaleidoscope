using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    internal class DefaultSessionProvider : ISessionProvider
    {
        public bool IsHeaderInjectionEnabled => false;

        public bool IsCookieInjectionEnabled => false;

        public Task<IEnumerable<KeyValuePair<string, string>>> GetHeaderAsync(Guid sessionIdentifier)
        {
            throw new NotSupportedException();
        }

        public Task SetCookieAsync(Guid sessionIdentifier, CookieContainer container)
        {
            throw new NotSupportedException();
        }
    }
}
