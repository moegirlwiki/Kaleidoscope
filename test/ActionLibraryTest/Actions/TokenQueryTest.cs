//-----------------------------------------------------------------------
// <copyright file="TokenQueryTest.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Actions;
using Moegirlpedia.MediaWikiInterop.Actions.QueryProviders;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System.Threading.Tasks;

namespace ActionLibraryTest.Actions
{
    [TestClass]
    public class TokenQueryTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public async Task GetToken()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {

                var queryResponse = await apiConnection.CreateAction<QueryAction>().RunActionAsync(config =>
                {
                    config.AddQueryProvider(new TokenQueryProvider
                    {
                        Types = TokenQueryProvider.TokenTypes.Login
                    });
                }, session);

                var tokenResponse = queryResponse.GetQueryTypedResponse<Tokens>();
                Assert.IsNotNull(tokenResponse?.Response?.LoginToken);
            }
        }

        [TestMethod]
        public async Task TestTokenRetrieve()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {
                var tokenQueryProvider = new TokenQueryProvider
                {
                    Types = TokenQueryProvider.TokenTypes.Csrf | TokenQueryProvider.TokenTypes.Login
                };

                // Send request
                var response = await apiConnection.CreateAction<QueryAction>().RunActionAsync(config =>
                {
                    Assert.IsNotNull(config);
                    config.AddQueryProvider(tokenQueryProvider);
                }, session);

                var parsedResult = response.GetQueryProviderResponse(tokenQueryProvider);

                Assert.IsNotNull(parsedResult);
                Assert.IsNotNull(parsedResult.Response);
                Assert.IsNull(parsedResult.ContinuationToken);

                // Tokens
                Assert.IsNotNull(parsedResult.Response.LoginToken);
            }
        }
    }
}
