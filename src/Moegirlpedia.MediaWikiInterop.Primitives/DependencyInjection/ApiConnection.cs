//-----------------------------------------------------------------------
// <copyright file="ApiConnection.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection
{
    public class ApiConnection : IDisposable
    {
        private IServiceScope m_serviceScope;
        private ISessionProvider m_sessionProvider;
        private ApiConnectionFactory m_parentFactory;
        private readonly IOptions<EnvironmentOption> m_envOptions;

        internal ApiConnection(ApiConnectionFactory factory)
        {
            m_parentFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            m_serviceScope = m_parentFactory.CreateScope();
            m_sessionProvider = m_serviceScope.ServiceProvider.GetRequiredService<ISessionProvider>();
            m_envOptions = m_serviceScope.ServiceProvider.GetRequiredService<IOptions<EnvironmentOption>>();
        }

        public Session CreateSession()
        {
            if (m_sessionProvider == null) throw new ObjectDisposedException(nameof(ApiConnection));
            return new Session(m_envOptions.Value.Endpoint, m_sessionProvider);
        }

        public Session CreateSessionFromId(Guid id)
        {
            if (m_sessionProvider == null) throw new ObjectDisposedException(nameof(ApiConnection));
            return new Session(m_envOptions.Value.Endpoint, m_sessionProvider, id);
        }

        public TAction CreateAction<TAction>()
            where TAction : IApiAction
        {
            if (m_serviceScope == null) throw new ObjectDisposedException(nameof(ApiConnection));
            return (TAction) Activator.CreateInstance(typeof(TAction), m_envOptions);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    m_serviceScope.Dispose();
                    m_sessionProvider.Dispose();
                }

                // TODO: set large fields to null.
                m_serviceScope = null;
                m_sessionProvider = null;
                m_parentFactory = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
