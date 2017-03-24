//-----------------------------------------------------------------------
// <copyright file="PurgeInputModel.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Actions.Transform;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moegirlpedia.MediaWikiInterop.Actions.Models
{
    public class PurgeInputModel : IApiActionRequest
    {
        [Required]
        public string Name { get; set; }

        [ApiParameter("forcelinkupdate")]
        public bool? ForceLinkUpdate { get; set; }

        [ApiParameter("forcerecursivelinkupdate")]
        public bool? ForceRecursiveLinkUpdate { get; set; }

        [ApiParameter("continue")]
        public string ContinuationToken { get; set; }

        [ApiParameter("titles")]
        public List<string> Titles { get; }

        [ApiParameter("pageids")]
        public List<int> PageIds { get; }

        [ApiParameter("revids")]
        public List<int> RevIds { get; }

        [ApiParameter("redirects")]
        public bool? ResolveRedirects { get; set; }

        [ApiParameter("converttitles")]
        public bool? ConvertTitles { get; set; }

        [ApiParameter("generator", false, true, typeof(GeneratorSerializer))]
        public Generator Generator { get; set; }
    }
}
