//-----------------------------------------------------------------------
// <copyright file="PurgeResponse.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    [ApiResponse("purge")]
    public class PurgeResponse : IApiActionResponse
    {
        [JsonProperty("purge")]
        public PurgedPage[] Purged { get; set; }

        [JsonProperty("normalized")]
        public NormalizedPage[] Normalized { get; set; }

        public class PurgedPage
        {
            [JsonProperty("ns")]
            public int NamespaceId { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("missing")]
            public bool IsMissing { get; set; } = false;

            [JsonProperty("purged")]
            public bool Purged { get; set; } = false;
        }

        public class NormalizedPage
        {
            [JsonProperty("fromencoded")]
            public bool FromEncoded { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }
        }
    }
}
