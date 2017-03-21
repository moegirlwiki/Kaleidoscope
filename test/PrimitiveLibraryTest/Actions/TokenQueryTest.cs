//-----------------------------------------------------------------------
// <copyright file="TokenQueryTest.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.Action;
using Moegirlpedia.MediaWikiInterop.Primitives.Action.QueryProviders;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System.Threading.Tasks;

namespace PrimitiveLibraryTest.Actions
{
    [TestClass]
    public class TokenQueryTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public async Task TestTokenRetrieve()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var actionPipe = apiConnection.CreateActionPipeline();
                var testSession = sessionFactory.CreateSession();
                var queryAction = actionPipe.CreateAction<QueryAction>();

                var tokenQueryProvider = new TokenQueryProvider
                {
                    Types = TokenQueryProvider.TokenTypes.Csrf | TokenQueryProvider.TokenTypes.Login
                };

                // Send request
                var response = await queryAction.RunActionAsync(config =>
                {
                    Assert.IsNotNull(config);
                    config.AddQueryProvider(tokenQueryProvider);
                }, testSession);

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
