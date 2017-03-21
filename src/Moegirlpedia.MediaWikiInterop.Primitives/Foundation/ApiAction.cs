//-----------------------------------------------------------------------
// <copyright file="ApiAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using Moegirlpedia.MediaWikiInterop.Primitives.Pipeline;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public abstract class ApiAction<TRequest> : IApiAction<TRequest> where TRequest : IApiActionRequest
    {
        private readonly ActionPipeline m_actionPipeline;

        public ApiAction(ActionPipeline actionPipeline)
        {
            m_actionPipeline = actionPipeline ?? throw new ArgumentNullException(nameof(actionPipeline));
        }

        public abstract string Name { get; }

        protected abstract Func<IRequestSerializer<TRequest>> SerializerAction { get; }

        public async Task RunActionAsync(Action<TRequest> configAction, 
            Session session,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (SerializerAction == null) throw new InvalidOperationException();

            // GateKeeper: Check sanity
            if (session.m_requestEndpoint != m_actionPipeline.m_envOptions.Value.Endpoint)
                throw new InvalidOperationException("Endpoint mismatch");

            // Third-party session provider: Set cookie now
            await session.SetCookieAsync(cancellationToken);

            // URL Builder
            var uriBuilder = new UriBuilder(session.m_requestEndpoint)
            {
                Query = $"action={WebUtility.UrlEncode(Name)}"
            };

            // Activate request instance
            var request = Activator.CreateInstance<TRequest>();
            configAction?.Invoke(request);

            // Get serializer. (Checked)
            var serializer = SerializerAction();

            using (var httpClientHandler = new HttpClientHandler { CookieContainer = session.m_cookieContainer })
            using (var httpClient = new HttpClient(httpClientHandler))
            // Third-party session provider: Set header after serialization
            using (var reqContent = await session.SetHeaderAsync(
                await serializer.SerializeRequestAsync(request, cancellationToken), cancellationToken))
            using (var response = await httpClient.PostAsync(uriBuilder.Uri, reqContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }

    public abstract class ApiAction<TRequest, TResponse> : IApiAction<TRequest, TResponse>
        where TRequest : IApiActionRequest
        where TResponse : IApiActionResponse
    {

        private readonly ActionPipeline m_actionPipeline;

        public ApiAction(ActionPipeline actionPipeline)
        {
            m_actionPipeline = actionPipeline ?? throw new ArgumentNullException(nameof(actionPipeline));
        }

        public abstract string Name { get; }

        protected abstract Func<IRequestSerializer<TRequest>> SerializerAction { get; }

        protected abstract Func<TRequest, IResponseDeserializer<TResponse>> DeserializerAction { get; }

        public async Task<TResponse> RunActionAsync(Action<TRequest> configAction,
            Session session,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (SerializerAction == null) throw new InvalidOperationException();
            if (DeserializerAction == null) throw new InvalidOperationException();

            // GateKeeper: Check sanity
            if (session.m_requestEndpoint != m_actionPipeline.m_envOptions.Value.Endpoint)
                throw new InvalidOperationException("Endpoint mismatch");

            // Third-party session provider: Set cookie now
            await session.SetCookieAsync(cancellationToken);

            // URL Builder
            var uriBuilder = new UriBuilder(session.m_requestEndpoint)
            {
                Query = $"action={WebUtility.UrlEncode(Name)}"
            };

            // Activate request instance
            var request = Activator.CreateInstance<TRequest>();
            configAction?.Invoke(request);

            // Get serializer. (Checked)
            var serializer = SerializerAction();

            // Get deserializer. (Checked)
            var deserializer = DeserializerAction(request);

            using (var httpClientHandler = new HttpClientHandler { CookieContainer = session.m_cookieContainer })
            using (var httpClient = new HttpClient(httpClientHandler))
            // Third-party session provider: Set header after serialization
            using (var reqContent = await session.SetHeaderAsync(
                await serializer.SerializeRequestAsync(request, cancellationToken), cancellationToken))
            using (var response = await httpClient.PostAsync(uriBuilder.Uri, reqContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                return await deserializer.DeserializeResponseAsync(response.Content, cancellationToken);
            }
        }
    }
}
