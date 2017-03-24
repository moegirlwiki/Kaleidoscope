//-----------------------------------------------------------------------
// <copyright file="IQueryProvider.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Actions.Query
{
    public interface IQueryProvider
    {
        List<KeyValuePair<string, string>> GetParameters();
    }

    public interface IQueryProvider<TResponse> : IQueryProvider
    {

    }
}
