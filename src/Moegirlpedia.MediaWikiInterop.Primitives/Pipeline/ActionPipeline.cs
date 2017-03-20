using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Pipeline
{
    public class ActionPipeline
    {
        private readonly IOptions<EnvironmentOption> m_envOptions;

        public ActionPipeline(IOptions<EnvironmentOption> envOptions)
        {
            m_envOptions = envOptions ?? throw new ArgumentNullException(nameof(envOptions));
        }

        public async Task<TResponse> RunActionAsync<TRequest, TResponse>(
            IApiAction<TRequest, TResponse> action, Session session, 
            CancellationToken ctkn = default(CancellationToken))
            where TRequest : IApiActionRequest
            where TResponse : IApiActionResponse
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (action == null) throw new ArgumentNullException(nameof(action));

            // GateKeeper: Check sanity
            if (session.m_requestEndpoint != m_envOptions.Value.Endpoint)
                throw new InvalidOperationException("Endpoint mismatch");

            // Third-party session provider: Set cookie now
            await session.SetCookieAsync(ctkn);

            // URL Builder
            var uriBuilder = new UriBuilder(session.m_requestEndpoint)
            {
                Query = $"action={WebUtility.UrlEncode(action.Name)}"
            };

            using (var httpClientHandler = new HttpClientHandler { CookieContainer = session.m_cookieContainer })
            using (var httpClient = new HttpClient(httpClientHandler))
            // Third-party session provider: Set header after serialization
            using (var reqContent = await session.SetHeaderAsync(
                await action.Serializer.Value.SerializeRequestAsync(action.Request, ctkn), ctkn))
            using (var response = await httpClient.PostAsync(uriBuilder.Uri, reqContent, ctkn))
            {
                response.EnsureSuccessStatusCode();
                return await action.Deserializer.Value.DeserializeResponseAsync(response.Content, ctkn);
            }
        }

        public async Task RunActionAsync<TRequest>(
            IApiAction<TRequest> action, Session session,
            CancellationToken ctkn = default(CancellationToken))
            where TRequest : IApiActionRequest
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (action == null) throw new ArgumentNullException(nameof(action));

            // GateKeeper: Check sanity
            if (session.m_requestEndpoint != m_envOptions.Value.Endpoint)
                throw new InvalidOperationException("Endpoint mismatch");

            // Third-party session provider: Set cookie now
            await session.SetCookieAsync(ctkn);

            // URL Builder
            var uriBuilder = new UriBuilder(session.m_requestEndpoint)
            {
                Query = $"action={WebUtility.UrlEncode(action.Name)}"
            };

            using (var httpClientHandler = new HttpClientHandler { CookieContainer = session.m_cookieContainer })
            using (var httpClient = new HttpClient(httpClientHandler))
            // Third-party session provider: Set header after serialization
            using (var reqContent = await session.SetHeaderAsync(
                await action.Serializer.Value.SerializeRequestAsync(action.Request, ctkn), ctkn))
            using (var response = await httpClient.PostAsync(uriBuilder.Uri, reqContent, ctkn))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
