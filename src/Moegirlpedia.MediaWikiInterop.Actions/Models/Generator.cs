//-----------------------------------------------------------------------
// <copyright file="Generator.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System.Collections.Generic;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    public class Generator : IApiActionRequest
    {
        [ApiParameter("gacfrom")]
        public string From { get; set; }

        [ApiParameter("gaccontinue")]
        public string ContinuationToken { get; set; }

        [ApiParameter("gacto")]
        public string To { get; set; }

        [ApiParameter("gacprefix")]
        public string Prefix { get; set; }

        [ApiParameter("gacdir", false, false, typeof(CamelCaseToLowerDashConverter))]
        public GeneratorDirection Direction { get; set; }

        [ApiParameter("gacmin")]
        public int? Min { get; set; }

        [ApiParameter("gacmax")]
        public int? Max { get; set; }

        [ApiParameter("gaclimit")]
        public int? Limit { get; set; }

        [ApiParameter("gacprop")]
        public List<string> Properties { get; set; }

        public enum GeneratorDirection
        {
            Ascending,
            Descending
        }
    }
}
