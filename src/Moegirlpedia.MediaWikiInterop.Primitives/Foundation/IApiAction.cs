using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    /// <summary>
    /// Interface for general, no response actions.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request model.</typeparam>
    public interface IApiAction<TRequest>
    {
        string Name { get; }
        IRequestSerializer<TRequest> Serializer { get; }
    }

    public interface IApiAction<TRequest, TResponse> : IApiAction<TRequest>
    {
        IResponseDeserializer<TResponse> Deserializer { get; }
    }
}
