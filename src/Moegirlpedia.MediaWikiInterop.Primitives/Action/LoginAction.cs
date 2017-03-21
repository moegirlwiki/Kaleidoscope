//-----------------------------------------------------------------------
// <copyright file="LoginAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Pipeline;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action
{
    public class LoginAction : ApiAction<LoginInputModel, LoginResponse>
    {
        public override string Name => "login";

        public LoginAction(ActionPipeline actionPipeline) : base(actionPipeline) { }

        protected override Func<LoginInputModel, IResponseDeserializer<LoginResponse>> DeserializerAction => 
            new Func<LoginInputModel, IResponseDeserializer<LoginResponse>>(
                req => new StandardResponseDeserializer<LoginResponse>());

        protected override Func<IRequestSerializer<LoginInputModel>> SerializerAction =>
            new Func<IRequestSerializer<LoginInputModel>>(() => 
            new FormUrlEncodedContentRequestSerializer<LoginInputModel>());
    }
}
