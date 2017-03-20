//-----------------------------------------------------------------------
// <copyright file="ApiError.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Foundation
{
    /// <summary>
    /// Actual data model class that represents MediaWiki API error.
    /// </summary>
    public class ApiError
    {
        public string Code { get; set; }
        public string Info { get; set; }
        public string DocRef { get; set; }
    }
}
