using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Extensions
{
    /// <summary>
    /// String extension method class for URL validation.
    /// </summary>
    public static class UrlValidationExtensions
    {
        public static bool ValidateUrl(this string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            var result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == "http" || uriResult.Scheme == "https");

            return result;
        }
    }
}
