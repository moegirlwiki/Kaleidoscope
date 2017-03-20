//-----------------------------------------------------------------------
// <copyright file="JsonFormatConfig.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    static class JsonFormatConfig
    {
        public const string MimeType = "application/json";

        public static readonly IReadOnlyList<KeyValuePair<string, string>> FormatConfig =
            new List<KeyValuePair<string, string>>
            {
                // Hard-code format information
                new KeyValuePair<string, string>("format", "json"),
                new KeyValuePair<string, string>("utf8", "1"),
                new KeyValuePair<string, string>("formatversion", "2")
            };
    }
}
