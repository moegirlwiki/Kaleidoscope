//-----------------------------------------------------------------------
// <copyright file="ActionPipeline.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Pipeline
{
    public class ActionPipeline
    {
        internal readonly IOptions<EnvironmentOption> m_envOptions;

        public ActionPipeline(IOptions<EnvironmentOption> envOptions)
        {
            m_envOptions = envOptions ?? throw new ArgumentNullException(nameof(envOptions));
        }

        public TAction CreateAction<TAction>()
            where TAction : IApiAction
        {
            return (TAction) Activator.CreateInstance(typeof(TAction), this);
        }
    }
}
