using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform.Internals;
using Newtonsoft.Json;
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
        public async Task<T> DeserializeResponseAsync(HttpContent input, CancellationToken ctkn)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (input)
            {
                if (input.Headers.ContentType?.MediaType != JsonFormatConfig.MimeType)
                    throw new ArgumentNullException("MIME type mismatch");

                var content = await input.ReadAsStringAsync();
                var jsonEntity = await Task.Factory.StartNew(() => GateKeeper.ParseJObject(content), ctkn);

                // Get Key
                var rAttrib = typeof(T).GetTypeInfo().GetCustomAttribute<ApiResponseAttribute>();
                var querySubKey = rAttrib?.Name;

                if (querySubKey == null) throw new InvalidOperationException("No key defined");

                var queryEntity = jsonEntity[querySubKey];
                if (queryEntity == null) throw new KeyNotFoundException(GateKeeper.PayloadNotFound);

                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(queryEntity.ToString()), ctkn);
            }
        }
    }
}
