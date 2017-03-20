//-----------------------------------------------------------------------
// <copyright file="IResponseDeserializer.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IResponseDeserializer<TResponse> where TResponse : IApiActionResponse
    {
        Task<TResponse> DeserializeResponseAsync(HttpContent input, CancellationToken ctkn = default(CancellationToken));
    }
}
