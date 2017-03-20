//-----------------------------------------------------------------------
// <copyright file="SessionFactory.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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

        public SessionFactory(ISessionProvider sessionProvider, IOptions<EnvironmentOption> envOptions)
        {
            m_envOptions = envOptions ?? throw new ArgumentNullException(nameof(envOptions));
            m_sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public Session CreateSession() => new Session(m_envOptions.Value.Endpoint, m_sessionProvider);
    }
}
