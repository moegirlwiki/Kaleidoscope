//-----------------------------------------------------------------------
// <copyright file="IApiAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    /// <summary>
    /// Interface for general, no response actions.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request model.</typeparam>
    public interface IApiAction<TRequest> where TRequest : IApiActionRequest
    {
        string Name { get; }
        Lazy<IRequestSerializer<TRequest>> Serializer { get; }
        TRequest Request { get; }
    }

    public interface IApiAction<TRequest, TResponse> : IApiAction<TRequest> 
        where TRequest : IApiActionRequest 
        where TResponse : IApiActionResponse
    {
        Lazy<IResponseDeserializer<TResponse>> Deserializer { get; }
    }
}
