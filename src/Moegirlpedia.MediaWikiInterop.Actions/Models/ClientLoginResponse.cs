using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    [ApiResponse("clientlogin")]
    public class ClientLoginResponse : IApiActionResponse
    {
        [JsonProperty("canpreservestate")]
        public bool? CanPreserveState { get; set; }

        [JsonProperty("status")]
        public AuthManagerLoginResult Status { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Status that represents auth manager login result.
        /// </summary>
        public enum AuthManagerLoginResult
        {
            Pass,
            Fail,
            UI,
            Redirect,
            Restart
        }

    }
}
