//-----------------------------------------------------------------------
// <copyright file="LoginAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action
{
    public class LoginAction : IApiAction<LoginInputModel, LoginResponse>
    {
        public string Name => "login";

        private Lazy<IRequestSerializer<LoginInputModel>> m_serializerInternal;
        private Lazy<IResponseDeserializer<LoginResponse>> m_deserializerInternal;

        public LoginAction(LoginInputModel model)
        {
            Request = model ?? throw new ArgumentNullException(nameof(model));

            m_serializerInternal = new Lazy<IRequestSerializer<LoginInputModel>>(() =>
            {
                return new FormUrlEncodedContentRequestSerializer<LoginInputModel>();
            });

            m_deserializerInternal = new Lazy<IResponseDeserializer<LoginResponse>>(() =>
            {
                return new StandardResponseDeserializer<LoginResponse>();
            });
        }

        public Lazy<IRequestSerializer<LoginInputModel>> Serializer => m_serializerInternal;

        public Lazy<IResponseDeserializer<LoginResponse>> Deserializer => m_deserializerInternal;

        public LoginInputModel Request { get; }
    }
}
