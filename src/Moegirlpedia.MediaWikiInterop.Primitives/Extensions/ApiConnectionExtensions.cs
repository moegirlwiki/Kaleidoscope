using Microsoft.Extensions.DependencyInjection;
using Moegirlpedia.MediaWikiInterop.Primitives.Identity;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Extensions
{
    public static class ApiConnectionExtensions
    {
        public static IServiceCollection AddSessionProvider<TService>(this IServiceCollection serviceCollection) 
            where TService : ISessionProvider
        {
            return serviceCollection?.AddScoped(typeof(ISessionProvider), typeof(TService)) 
                ?? throw new ArgumentNullException(nameof(serviceCollection));
        }
    }
}
