//-----------------------------------------------------------------------
// <copyright file="LoginResponse.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    [ApiResponse("login")]
    public class LoginResponse : IApiActionResponse
    {
        [JsonProperty("result")]
        public LoginResult Result { get; set; }

        [JsonProperty("lguserid")]
        public int UserId { get; set; }

        [JsonProperty("lgusername")]
        public string Username { get; set; }

        public enum LoginResult
        {
            NeedToken,
            WrongToken,
            Failed,
            Aborted,
            Success
        }
    }
}
