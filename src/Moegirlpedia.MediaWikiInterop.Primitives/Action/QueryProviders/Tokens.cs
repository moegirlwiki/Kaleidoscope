//-----------------------------------------------------------------------
// <copyright file="Tokens.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action.QueryProviders
{
    public class Tokens
    {
        [JsonProperty("logintoken")]
        public string LoginToken { get; set; }

        [JsonProperty("createaccounttoken")]
        public string CreateAccountToken { get; set; }

        [JsonProperty("csrftoken")]
        public string CsrfToken { get; set; }

        [JsonProperty("patroltoken")]
        public string PatrolToken { get; set; }

        [JsonProperty("rollbacktoken")]
        public string RollbackToken { get; set; }

        [JsonProperty("userrightstoken")]
        public string UserRightsToken { get; set; }

        [JsonProperty("watchtoken")]
        public string WatchToken { get; set; }
    }
}
