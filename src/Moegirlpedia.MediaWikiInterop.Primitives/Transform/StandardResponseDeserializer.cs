using Moegirlpedia.MediaWikiInterop.Primitives.Exceptions;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class StandardResponseDeserializer<T> : IResponseDeserializer<T> where T : IApiActionResponse
    {
        private const string MimeType = "application/json";
        private const string ErrorKey = "error";
        private const string PayloadParseFailureGeneral = "Unable to parse response payload.";
        private const string PayloadNotFound = "The specified query sub entity is not found.";

        /// <summary>
        /// Internal gate keeper that checks response error.
        /// </summary>
        /// <param name="initialObject">Instance of <see cref="JObject"/></param>
        /// <exception cref="InvalidOperationException">Thrown if the given entity is null.</exception>
        /// <exception cref="MediaWikiApiInvocationException">Thrown if remote API error is found.</exception>
        private static void GateKeeperCheckError(JObject initialObject)
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
        private static JObject GateKeeperParseJObject(string jsonResponse, bool ignoreApiErrorCheck = false)
        {
            // No sanity check - caller path is determined
            var jsonEntity = JObject.Parse(jsonResponse);
            if (!ignoreApiErrorCheck) GateKeeperCheckError(jsonEntity);

            return jsonEntity;
        }

        public async Task<T> DeserializeResponseAsync(HttpContent input, CancellationToken ctkn)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.Headers.ContentType?.MediaType != MimeType)
                throw new ArgumentNullException("MIME type mismatch");

            var content = await input.ReadAsStringAsync();
            var jsonEntity = await Task.Factory.StartNew(() => GateKeeperParseJObject(content), ctkn);

            // Get Key
            var rAttrib = typeof(T).GetTypeInfo().GetCustomAttribute<ApiResponseAttribute>();
            var querySubKey = rAttrib?.Name;

            if (querySubKey == null) throw new InvalidOperationException("No key defined");

            var queryEntity = jsonEntity[querySubKey];
            if (queryEntity == null) throw new KeyNotFoundException(PayloadNotFound);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(queryEntity.ToString()), ctkn);
        }
    }
}
