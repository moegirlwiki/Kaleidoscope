using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public class SessionFactory
    {
        private readonly IOptions<EnvironmentOption> m_envOptions;
        private readonly ISessionProvider m_sessionProvider;

        internal SessionFactory(ISessionProvider sessionProvider, IOptions<EnvironmentOption> envOptions)
        {
            m_envOptions = envOptions ?? throw new ArgumentNullException(nameof(envOptions));
            m_sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public MwSession CreateSession() => new MwSession(m_envOptions.Value.Endpoint, m_sessionProvider);
    }
}
