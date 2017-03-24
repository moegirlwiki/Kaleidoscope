//-----------------------------------------------------------------------
// <copyright file="PurgeAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;

namespace Moegirlpedia.MediaWikiInterop.Actions
{
    public class PurgeAction : ApiAction<PurgeInputModel, PurgeResponse>
    {
        public override string Name => "purge";

        public PurgeAction(IOptions<EnvironmentOption> envOptions) : base(envOptions) { }

        protected override Func<PurgeInputModel, IResponseDeserializer<PurgeResponse>> DeserializerAction =>
            new Func<PurgeInputModel, IResponseDeserializer<PurgeResponse>>(
                req => new StandardResponseDeserializer<PurgeResponse>());

        protected override Func<IRequestSerializer<PurgeInputModel>> SerializerAction =>
            new Func<IRequestSerializer<PurgeInputModel>>(() =>
            new FormUrlEncodedContentRequestSerializer<PurgeInputModel>());
    }
}
