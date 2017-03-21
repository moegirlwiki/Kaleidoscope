//-----------------------------------------------------------------------
// <copyright file="IApiAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    public interface IApiAction
    {
        string Name { get; }
    }

    /// <summary>
    /// Interface for general, no response actions.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request model.</typeparam>
    public interface IApiAction<TRequest> : IApiAction where TRequest : IApiActionRequest
    {
        
    }

    public interface IApiAction<TRequest, TResponse> : IApiAction<TRequest> 
        where TRequest : IApiActionRequest 
        where TResponse : IApiActionResponse
    {
    }
}
