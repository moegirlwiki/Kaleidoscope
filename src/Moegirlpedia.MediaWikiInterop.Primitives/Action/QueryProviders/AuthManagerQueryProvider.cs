using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Query;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Action.QueryProviders
{
    [QueryProvider("authmanagerinfo", "meta", typeof(AuthManagerInfo))]
    public class AuthManagerQueryProvider : IQueryProvider<AuthManagerInfo>
    {
        [ApiParameter("amisecuritysensitiveoperation")]
        public string IsSensitive { get; set; }

        [ApiParameter("amimergerequestfields")]
        public bool? MergeFields { get; set; }

        [ApiParameter("amirequestsfor", false, false, typeof(CamelCaseToLowerDashConverter))]
        public AmiReqTypes? RequestFor { get; set; }

        [ApiParameter("amimessageformat", false, false, typeof(CamelCaseToLowerDashConverter))]
        public AmiMessageFormat? MessageFormat { get; set; }

        public List<KeyValuePair<string, string>> GetParameters()
        {
            return this.ToKeyValuePairCollection();
        }

        public enum AmiReqTypes
        {
            Login,
            LoginContinue,
            Create,
            CreateContinue,
            Link,
            LinkContinue,
            Change,
            Remove,
            Unlink
        }

        public enum AmiMessageFormat
        {
            WikiText,
            Html,
            Raw,
            None
        }
    }
}
