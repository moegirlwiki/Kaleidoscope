//-----------------------------------------------------------------------
// <copyright file="TokenQueryTest.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.Action;
using Moegirlpedia.MediaWikiInterop.Primitives.Action.QueryProviders;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimitiveLibraryTest.Actions
{
    [TestClass]
    public class TokenQueryTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public void TestTokenRetrievePreRequest()
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

                // Determined
                var serializedContent = ((QueryRequestSerializer)queryAction.Serializer.Value).GetKeyPairs(queryAction.Request);
                Assert.AreNotEqual(0, serializedContent.Count);

                // Confirm keys
                Assert.AreEqual(true, serializedContent.Contains(new KeyValuePair<string, string>("rawcontinue", "1")));
                Assert.AreEqual(true, serializedContent.Contains(new KeyValuePair<string, string>("meta", "tokens")));
                Assert.AreEqual(true, serializedContent.Contains(new KeyValuePair<string, string>("format", "json")));
                Assert.AreEqual(true, serializedContent.Contains(new KeyValuePair<string, string>("type", "csrf|login")));
            }
        }

        [TestMethod]
        public async Task TestTokenRetrieve()
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

                // Send request
                var response = await actionPipe.RunActionAsync(queryAction, testSession);
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
