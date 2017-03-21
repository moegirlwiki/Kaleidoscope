//-----------------------------------------------------------------------
// <copyright file="ApiConnectionFactory.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moegirlpedia.MediaWikiInterop.Primitives.Extensions;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection
{
    public class ApiConnectionFactory
    {
        private IServiceCollection m_serviceCollection;
        private IServiceProvider m_serviceProvider;
        private IConfigurationRoot m_configuration;

        internal bool m_hasThirdPartySessionProvider;

        private ApiConnectionFactory(string endpoint)
        {
            if (!endpoint.ValidateUrl()) throw new ArgumentException("Invalid API endpoint");

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("EnvironmentOption:Endpoint", endpoint)
                });

            m_configuration = builder.Build();
            m_serviceCollection = new ServiceCollection();
            m_hasThirdPartySessionProvider = false;
        }

        private void ConfigureServices()
        {
            // Add options support.
            m_serviceCollection.AddOptions();

            // Add logging support.
            m_serviceCollection.AddLogging();

            // Configure environment settings.
            m_serviceCollection.Configure<EnvironmentOption>(m_configuration.GetSection(nameof(EnvironmentOption)));
        }

        private void Configure()
        {
            // Currently nothing to do here
        }

        private void CheckSessionProvider()
        {
            if (!m_serviceCollection.Where(i => i.ServiceType == typeof(ISessionProvider)).Any())
            {
                // Add default session provider if a 3P is not present
                m_serviceCollection.AddScoped<ISessionProvider, DefaultSessionProvider>();
            }
        }

        internal IServiceScope CreateScope() => m_serviceProvider.CreateScope();

        public ApiConnection CreateConnection() => new ApiConnection(this);

        public static ApiConnectionFactory CreateConnection(
            string endpoint, Action<IServiceCollection> addserviceAction = null,
            Action<IServiceProvider> configAction = null)
        {
            var ret = new ApiConnectionFactory(endpoint);

            // Configure faciliy services.
            ret.ConfigureServices();

            // Inject other services (e.g. Session Provider if needed)
            addserviceAction?.Invoke(ret.m_serviceCollection);

            // Check critical components
            ret.CheckSessionProvider();

            // Build service provider.
            ret.m_serviceProvider = ret.m_serviceCollection.BuildServiceProvider();

            // Configure services.
            ret.Configure();

            // Configure services if required
            configAction?.Invoke(ret.m_serviceProvider);

            return ret;
        }
    }
}
