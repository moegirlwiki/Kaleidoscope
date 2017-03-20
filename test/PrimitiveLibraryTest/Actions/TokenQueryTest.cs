using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.Action;
using Moegirlpedia.MediaWikiInterop.Primitives.Action.QueryProviders;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveLibraryTest.Actions
{
    [TestClass]
    public class TokenQueryTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public async Task TestTokenRetrievePreRequest()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var actionPipe = apiConnection.CreateActionPipeline();

                var testSession = sessionFactory.CreateSession();
                var tokenQueryProvider = new TokenQueryProvider
                {
                    Types = TokenQueryProvider.TokenTypes.Csrf | TokenQueryProvider.TokenTypes.Login
                };
                var queryAction = new QueryAction();

                Assert.IsNotNull(queryAction.Request);
                queryAction.Request.AddQueryProvider(tokenQueryProvider);

                var serializedContent = await queryAction.Serializer.Value.SerializeRequestAsync(queryAction.Request);
            }
        }
    }
}
