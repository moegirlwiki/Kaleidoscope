//-----------------------------------------------------------------------
// <copyright file="Session.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public class Session : IDisposable
    {
        internal readonly string m_requestEndpoint;
        private ISessionProvider m_sessionProvider;
        internal CookieContainer m_cookieContainer;

        public Guid SessionIdentifier { get; }

        internal Session(string requestEndpoint, ISessionProvider sessionProvider, 
            Guid identifier)
        {
            SessionIdentifier = identifier;

            m_requestEndpoint = requestEndpoint;
            m_sessionProvider = sessionProvider;
            m_cookieContainer = new CookieContainer();
        }

        internal Session(string requestEndpoint, ISessionProvider sessionProvider)
        {
            SessionIdentifier = Guid.NewGuid();

            m_requestEndpoint = requestEndpoint;
            m_sessionProvider = sessionProvider;
            m_cookieContainer = new CookieContainer();
        }

        public async Task SetCookieAsync(CancellationToken ctkn = default(CancellationToken))
        {
            if (m_sessionProvider == null) throw new ObjectDisposedException(nameof(Session));

            if (m_sessionProvider.IsCookieInjectionEnabled)
                await m_sessionProvider.SetCookieAsync(SessionIdentifier, m_cookieContainer, ctkn);
        }

        public async Task<HttpContent> SetHeaderAsync(HttpContent reqContent, CancellationToken ctkn = default(CancellationToken))
        {
            if (m_sessionProvider == null) throw new ObjectDisposedException(nameof(Session));

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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // We will not dispose session provider for the sake of connection reuse.
                    // However, we will deference it.
                }

                m_cookieContainer = null;
                m_sessionProvider = null;

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
