using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public class Session
    {
        private readonly string m_requestEndpoint;
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
    }
}
