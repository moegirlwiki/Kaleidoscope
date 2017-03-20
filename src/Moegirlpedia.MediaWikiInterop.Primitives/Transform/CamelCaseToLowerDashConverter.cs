//-----------------------------------------------------------------------
// <copyright file="CamelCaseToLowerDashConverter.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System.Text.RegularExpressions;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class CamelCaseToLowerDashConverter : IFinalizedApiParamSerializer
    {
        private static Regex MatchingRegex = new Regex("([a-zA-Z])(?=[A-Z])");

        public string SerializeFinalizedFields(object entity)
        {
            if (entity == null) return string.Empty;
            var camelString = entity.ToString();
            camelString = MatchingRegex.Replace(camelString, new MatchEvaluator(match => $"{match}-"));

            return camelString.ToLower();
        }
    }
}
