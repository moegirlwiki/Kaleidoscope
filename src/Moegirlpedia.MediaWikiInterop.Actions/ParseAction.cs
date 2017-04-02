using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Actions
{
    public class ParseAction : ApiAction<PageParseInputModel, ParseResponse>
    {
        public ParseAction(IOptions<EnvironmentOption> envOptions) : base(envOptions)
        {
        }

        public override string Name => "parse";

        protected override Func<IRequestSerializer<PageParseInputModel>> SerializerAction => 
            new Func<IRequestSerializer<PageParseInputModel>>(() => new PageParseActionSerializer());

        protected override Func<PageParseInputModel, IResponseDeserializer<ParseResponse>> DeserializerAction =>
            new Func<PageParseInputModel, IResponseDeserializer<ParseResponse>>(i =>
            new PageParseActionDeserializer());

        public class PageParseActionSerializer : IRequestSerializer<PageParseInputModel>
        {
            public Task<HttpContent> SerializeRequestAsync(
                PageParseInputModel content, 
                CancellationToken ctkn = default(CancellationToken))
            {
                return Task.Run<HttpContent>(() =>
                {
                    if (content == null) throw new ArgumentNullException(nameof(content));

                    var preSerializedFields = content.ToKeyValuePairCollection();

                    // Some sanity check
                    if (preSerializedFields.Count < 1) throw new InvalidOperationException("No page or ID is specified to parse.");

                    // Check other settings
                    if (content.MobileSettings != null) preSerializedFields.AddRange(content.MobileSettings.ToKeyValuePairCollection());
                    if (content.OutputSettings != null) preSerializedFields.AddRange(content.OutputSettings.ToKeyValuePairCollection());

                    // Format
                    preSerializedFields.AddRange(JsonFormatConfig.FormatConfig);

                    // Return fields
                    return new FormUrlEncodedContent(preSerializedFields);

                }, ctkn);
            }
        }

        public class PageParseActionDeserializer : IResponseDeserializer<ParseResponse>
        {
            public async Task<ParseResponse> DeserializeResponseAsync(
                HttpContent input, 
                CancellationToken ctkn = default(CancellationToken))
            {
                var stdDeserializer = new StandardResponseDeserializer<ParseResponse>();
                var entity = await stdDeserializer.DeserializeResponseWithRawObjectAsync(input, ctkn);
                entity.Item1.SetRawData(entity.Item2);

                return entity.Item1;
            }
        }
    }
}
