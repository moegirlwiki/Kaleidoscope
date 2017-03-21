//-----------------------------------------------------------------------
// <copyright file="QueryAction.cs" company="Project Kaleidoscope Authors">
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
    public class QueryAction : ApiAction<QueryInputModel, QueryResponse>
    {
        public QueryAction(ActionPipeline actionPipeline) : base(actionPipeline) { }

        public override string Name => "query";

        protected override Func<QueryInputModel, IResponseDeserializer<QueryResponse>> DeserializerAction => 
            new Func<QueryInputModel, IResponseDeserializer<QueryResponse>>(req =>
            new QueryResponseDeserializer(req));

        protected override Func<IRequestSerializer<QueryInputModel>> SerializerAction => 
            new Func<IRequestSerializer<QueryInputModel>>(() =>
            new QueryRequestSerializer());
    }
}
