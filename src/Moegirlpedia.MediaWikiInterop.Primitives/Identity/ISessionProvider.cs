//-----------------------------------------------------------------------
// <copyright file="ISessionProvider.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Identity
{
    public interface ISessionProvider : IDisposable
    {
        bool IsHeaderInjectionEnabled { get; }
        bool IsCookieInjectionEnabled { get; }
        
        Task<IEnumerable<KeyValuePair<string, string>>> GetHeaderAsync(Guid sessionIdentifier, CancellationToken ctkn);

        Task SetCookieAsync(Guid sessionIdentifier, CookieContainer container, CancellationToken ctkn);
    }
}
