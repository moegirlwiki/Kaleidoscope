using Microsoft.Extensions.Options;
using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation;
using Moegirlpedia.MediaWikiInterop.Primitives.Foundation.Internals;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Moegirlpedia.MediaWikiInterop.Actions
{
    public class ClientLoginAction : ApiAction<ClientLoginInputModel, ClientLoginResponse>
    {
        public ClientLoginAction(IOptions<EnvironmentOption> envOptions) : base(envOptions)
        {

        }

        public override string Name => "clientlogin";

        protected override Func<IRequestSerializer<ClientLoginInputModel>> SerializerAction => 
            new Func<IRequestSerializer<ClientLoginInputModel>>(() => new ClientLoginActionSerializer());

        protected override Func<ClientLoginInputModel, IResponseDeserializer<ClientLoginResponse>> DeserializerAction => 
            new Func<ClientLoginInputModel, IResponseDeserializer<ClientLoginResponse>>(
                t => new StandardResponseDeserializer<ClientLoginResponse>());

        protected class ClientLoginActionSerializer : IRequestSerializer<ClientLoginInputModel>
        {
            public Task<HttpContent> SerializeRequestAsync(
                ClientLoginInputModel content, 
                CancellationToken ctkn = default(CancellationToken))
            {
                return Task.Run<HttpContent>(() =>
                {
                    if (content == null) throw new ArgumentNullException(nameof(content));

                    var preSerializedFields = content.ToKeyValuePairCollection();

                    // Add format config
                    preSerializedFields.AddRange(JsonFormatConfig.FormatConfig);

                    // Join other fields
                    preSerializedFields.AddRange(
                        content.m_loginFields.Select(
                            i => new KeyValuePair<string, string>(i.Item1, i.Item2)));

                    // Return fields
                    return new FormUrlEncodedContent(preSerializedFields);
                }, ctkn);
            }
        }
    }
}
