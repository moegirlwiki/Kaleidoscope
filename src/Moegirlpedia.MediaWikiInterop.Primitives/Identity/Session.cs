using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public class Session
    {
        internal readonly string m_requestEndpoint;
        private readonly ISessionProvider m_sessionProvider;
        internal readonly CookieContainer m_cookieContainer;

        public Guid SessionIdentifier { get; }

        internal Session(string requestEndpoint, ISessionProvider sessionProvider)
        {
            SessionIdentifier = Guid.NewGuid();

            m_requestEndpoint = requestEndpoint;
            m_sessionProvider = sessionProvider;
            m_cookieContainer = new CookieContainer();
        }

        public async Task SetCookieAsync(CancellationToken ctkn)
        {
            if (m_sessionProvider.IsCookieInjectionEnabled)
                await m_sessionProvider.SetCookieAsync(SessionIdentifier, m_cookieContainer, ctkn);
        }

        public async Task<HttpContent> SetHeaderAsync(HttpContent reqContent, CancellationToken ctkn)
        {
            if (m_sessionProvider.IsHeaderInjectionEnabled)
            {
                var headers = await m_sessionProvider.GetHeaderAsync(SessionIdentifier, ctkn);
                if (reqContent != null)
                {
                    foreach (var h in headers) reqContent.Headers.Add(h.Key, h.Value);
                }
            }

            return reqContent;
        }
    }
}
