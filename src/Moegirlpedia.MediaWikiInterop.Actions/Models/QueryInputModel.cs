//-----------------------------------------------------------------------
// <copyright file="QueryInputModel.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Query;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    public class QueryInputModel : IApiActionRequest
    {
        internal List<IQueryProvider> m_queryProviders;

        public QueryInputModel()
        {
            m_queryProviders = new List<IQueryProvider>();
        }

        public void AddQueryProvider(IQueryProvider provider)
        {
            // Verify attribute if correctly set
            if (!provider.GetType().GetTypeInfo().IsDefined(typeof(QueryProviderAttribute)))
            {
                throw new InvalidOperationException("No query provider metadata is available");
            }
            // Add query provider
            m_queryProviders.Add(provider);
        }

        [ApiParameter("indexpageids")]
        public bool? IncludeIndexPageIds { get; set; }

        [ApiParameter("export")]
        public bool? Export { get; set; }

        [ApiParameter("exportnowrap")]
        public bool? ExportNoWrap { get; set; }

        [ApiParameter("iwurl")]
        public bool? IwUrl { get; set; }

        [ApiParameter("titles")]
        public List<string> Titles { get; }

        [ApiParameter("pageids")]
        public List<int> PageIds { get; }

        [ApiParameter("revids")]
        public List<int> RevIds { get; }

        [ApiParameter("redirects")]
        public bool? Redirects { get; }
    }
}
