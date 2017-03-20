using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Exceptions
{
    /// <summary>
    /// Exception that represents MediaWiki API invocation failure.
    /// </summary>
    public class MediaWikiApiInvocationException : Exception
    {
        public ApiError MwError { get; }

        /// <summary>
        /// Class constructor that creates instance of <see cref="MediaWikiApiInvocationException"/>.
        /// </summary>
        /// <param name="mediaWikiError">Instance of <see cref="ApiError"/>.</param>
        public MediaWikiApiInvocationException(ApiError mediaWikiError) : base(mediaWikiError?.Info)
        {
            MwError = mediaWikiError;
        }
    }
}
