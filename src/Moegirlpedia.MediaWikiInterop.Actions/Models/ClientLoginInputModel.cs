using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    public class ClientLoginInputModel : IApiActionRequest
    {
        internal readonly List<(string, string)> m_loginFields;

        public ClientLoginInputModel()
        {
            Requests = new List<string>();
            m_loginFields = new List<(string, string)>(10);
        }

        [ApiParameter("loginrequests", true)]
        public List<string> Requests { get; }

        [ApiParameter("loginmessageformat", true, false, typeof(CamelCaseToLowerDashConverter))]
        public LoginMessageFormat MessageFormat { get; set; }

        [ApiParameter("loginmergerequestfields", true)]
        public bool? MergeFields { get; set; }

        [ApiParameter("loginpreservestate", true)]
        public bool? PreserveState { get; set; }

        [ApiParameter("loginreturnurl", true)]
        public string ReturnUrl { get; set; }

        [ApiParameter("logincontinue", true)]
        public bool? IsContinuation { get; set; }

        [Required]
        [ApiParameter("logintoken", true)]
        public string LoginToken { get; set; }

        public void AddCredential(string key, string content)
        {
            m_loginFields.Add((key, content));
        }

        public enum LoginMessageFormat
        {
            Html,
            Wikitext,
            Raw,
            None
        }
    }
}
