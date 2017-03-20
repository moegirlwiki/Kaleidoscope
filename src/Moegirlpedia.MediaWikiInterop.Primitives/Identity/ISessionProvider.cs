using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public interface ISessionProvider
    {
        bool IsHeaderInjectionEnabled { get; }
        bool IsCookieInjectionEnabled { get; }
        
        Task<IEnumerable<KeyValuePair<string, string>>> GetHeaderAsync(Guid sessionIdentifier);

        Task SetCookieAsync(Guid sessionIdentifier, CookieContainer container);
    }
}
