using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    public class ParseInputBaseModel : IApiActionRequest
    {
        protected readonly HashSet<ParseProp> m_props;

        public OutputSettingsInputModel OutputSettings { get; set; }
        public MobileFrontendParseInputModel MobileSettings { get; set; }

        public ParseInputBaseModel()
        {
            m_props = new HashSet<ParseProp>();
        }

        public void AddParseProp(ParseProp prop)
        {
            m_props.Add(prop);
        }

        public void RemoveParseProp(ParseProp prop)
        {
            m_props.Remove(prop);
        }
    }

    public class PageParseInputModel : ParseInputBaseModel
    {
        [ApiParameter("pageid")]
        public int? PageId { get; set; }

        [ApiParameter("oldid")]
        public int? OldId { get; set; }

        [ApiParameter("page")]
        public string Page { get; set; }

        [ApiParameter("redirects")]
        public bool? ResolveRedirects { get; set; }
    }

    public class OutputSettingsInputModel : IApiActionRequest
    {

        [ApiParameter("disablelimitreport")]
        public bool? SuppressLimitReport { get; set; }

        [ApiParameter("disableeditsection")]
        public bool? SuppressEditSection { get; set; }

        [ApiParameter("disabletidy")]
        public bool? SuppressTidy { get; set; }

        [ApiParameter("preview")]
        public bool? PreviewMode { get; set; }

        [ApiParameter("sectionpreview")]
        public bool? SectionPreviewMode { get; set; }

        [ApiParameter("disabletoc")]
        public bool? SuppressToc { get; set; }

    }

    public class MobileFrontendParseInputModel : IApiActionRequest
    {
        [ApiParameter("mainpage")]
        public bool? ApplyMobileMainpageTransform { get; set; }

        [ApiParameter("noimages")]
        public bool? SuppressImages { get; set; }

        [ApiParameter("mobileformat")]
        public bool? IsMobileFormat { get; set; }
    }

    public enum ParseProp
    {
        Text,
        Langlinks,
        Categories,
        CategoriesHtml,
        Links,
        Templates,
        Images,
        ExternalLinks,
        Sections,
        RevId,
        DisplayTitle,
        HeadItems,
        HeadHtml,
        Modules,
        JsConfigVars,
        EncodedJsConfigVars,
        Indicators,
        IwLinks,
        WikiText,
        Properties,
        LimitReportData,
        LimitReportHtml,
        ParseTree
    }
}
