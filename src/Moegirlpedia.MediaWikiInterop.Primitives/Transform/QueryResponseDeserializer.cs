using Moegirlpedia.MediaWikiInterop.Primitives.Action.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform.Internals;
using System.Reflection;

namespace Moegirlpedia.MediaWikiInterop.Primitives.Transform
{
    public class QueryResponseDeserializer : IResponseDeserializer<QueryResponse>
    {
        private readonly QueryInputModel m_inputModel;

        public QueryResponseDeserializer(QueryInputModel inputModel)
        {
            m_inputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }

        public async Task<QueryResponse> DeserializeResponseAsync(HttpContent input, CancellationToken ctkn)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.Headers.ContentType?.MediaType != JsonFormatConfig.MimeType)
                throw new ArgumentNullException("MIME type mismatch");

            var content = await input.ReadAsStringAsync();
            var jsonEntity = await Task.Factory.StartNew(() => GateKeeper.ParseJObject(content), ctkn);

            // Get Key
            var rAttrib = typeof(QueryResponse).GetTypeInfo().GetCustomAttribute<ApiResponseAttribute>();
            var querySubKey = rAttrib?.Name;

            if (querySubKey == null) throw new InvalidOperationException("No key defined");

            var queryEntity = jsonEntity[querySubKey];
            if (queryEntity == null) throw new KeyNotFoundException(GateKeeper.PayloadNotFound);

            // Get continuation tokens


            throw new NotImplementedException();
        }
    }
}
