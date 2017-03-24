//-----------------------------------------------------------------------
// <copyright file="LogoutAction.cs" company="Project Kaleidoscope Authors">
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
    public class LogoutAction : ApiAction<LogoutInputModel>
    {
        public override string Name => "logout";

        public LogoutAction(IOptions<EnvironmentOption> envOptions) : base(envOptions) { }

        protected override Func<IRequestSerializer<LogoutInputModel>> SerializerAction =>
            new Func<IRequestSerializer<LogoutInputModel>>(() =>
            new FormUrlEncodedContentRequestSerializer<LogoutInputModel>());
    }
}
