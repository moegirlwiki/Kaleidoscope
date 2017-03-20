//-----------------------------------------------------------------------
// <copyright file="QueryAction.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action
{
    public class QueryAction : IApiAction<QueryInputModel, QueryResponse>
    {
        private QueryInputModel m_request;

        public QueryAction()
        {
            m_request = new QueryInputModel();
        }

        public Lazy<IResponseDeserializer<QueryResponse>> Deserializer => 
            new Lazy<IResponseDeserializer<QueryResponse>>(() =>
            new QueryResponseDeserializer(m_request));

        public string Name => "query";

        public Lazy<IRequestSerializer<QueryInputModel>> Serializer => 
            new Lazy<IRequestSerializer<QueryInputModel>>(() => 
            new QueryRequestSerializer());

        public QueryInputModel Request => m_request;
    }
}
