//-----------------------------------------------------------------------
// <copyright file="GateKeeper.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Moegirlpedia.MediaWikiInterop.Primitives.Exceptions;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform.Internals
{
    public class GateKeeper
    {
        
        public const string ErrorKey = "error";
        public const string PayloadParseFailureGeneral = "Unable to parse response payload.";
        public const string PayloadNotFound = "The specified query sub entity is not found.";

        /// <summary>
        /// Internal gate keeper that checks response error.
        /// </summary>
        /// <param name="initialObject">Instance of <see cref="JObject"/></param>
        /// <exception cref="InvalidOperationException">Thrown if the given entity is null.</exception>
        /// <exception cref="MediaWikiApiInvocationException">Thrown if remote API error is found.</exception>
        public static void CheckError(JObject initialObject)
        {
            if (initialObject == null) throw new InvalidOperationException(PayloadParseFailureGeneral);

            var errorSection = initialObject[ErrorKey];
            if (errorSection != null)
            {
                var mwError = JsonConvert.DeserializeObject<ApiError>(errorSection.ToString());
                throw new MediaWikiApiInvocationException(mwError);
            }
        }

        /// <summary>
        /// Internal helper that parses and checks response.
        /// </summary>
        /// <param name="jsonResponse">Raw JSON input.</param>
        /// <returns>Instance of <see cref="JObject"/>.</returns>
        /// <exception cref="MediaWikiApiInvocationException">Thrown if remote API error is found.</exception>
        public static JObject ParseJObject(string jsonResponse, bool ignoreApiErrorCheck = false)
        {
            // No sanity check - caller path is determined
            var jsonEntity = JObject.Parse(jsonResponse);
            if (!ignoreApiErrorCheck) CheckError(jsonEntity);

            return jsonEntity;
        }
    }
}
