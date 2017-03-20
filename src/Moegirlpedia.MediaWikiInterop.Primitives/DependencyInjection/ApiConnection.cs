using Microsoft.Extensions.DependencyInjection;
using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using Moegirlpedia.MediaWikiInterop.Primitives.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection
{
    public class ApiConnection : IDisposable
    {
        private IServiceScope m_serviceScope;
        private ApiConnectionFactory m_parentFactory;

        internal ApiConnection(ApiConnectionFactory factory)
        {
            m_parentFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            m_serviceScope = m_parentFactory.CreateScope();
        }

        public SessionFactory CreateSessionFactory()
        {
            if (m_serviceScope == null) throw new ObjectDisposedException(nameof(ApiConnection));
            return m_serviceScope.ServiceProvider.GetRequiredService<SessionFactory>();
        }

        public ActionPipeline CreateActionPipeline()
        {
            if (m_serviceScope == null) throw new ObjectDisposedException(nameof(ApiConnection));
            return m_serviceScope.ServiceProvider.GetRequiredService<ActionPipeline>();
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
                }

                // TODO: set large fields to null.
                m_serviceScope = null;
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
