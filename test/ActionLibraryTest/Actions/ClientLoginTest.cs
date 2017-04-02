using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Actions;
using Moegirlpedia.MediaWikiInterop.Actions.Models;
using Moegirlpedia.MediaWikiInterop.Actions.QueryProviders;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ActionLibraryTest.Actions
{
    [TestClass]
    public class ClientLoginTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public async Task TestClientLogin()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {
                // First, token
                var queryResponse = await apiConnection.CreateAction<QueryAction>().RunActionAsync(config =>
                {
                    config.AddQueryProvider(new TokenQueryProvider
                    {
                        Types = TokenQueryProvider.TokenTypes.Login
                    });
                }, session);

                var tokenResponse = queryResponse.GetQueryTypedResponse<Tokens>();
                Assert.IsNotNull(tokenResponse?.Response?.LoginToken);

                // Then, login
                var loginResponse = await apiConnection.CreateAction<ClientLoginAction>().RunActionAsync(config =>
                {

                    config.LoginToken = tokenResponse.Response.LoginToken;
                    config.ReturnUrl = "https://zh.moegirl.org/Mainpage";
                    config.AddCredential("username", Environment.GetEnvironmentVariable("UT_USERNAME"));
                    config.AddCredential("password", Environment.GetEnvironmentVariable("UT_PASSWORD"));

                }, session);

                Assert.AreEqual(ClientLoginResponse.AuthManagerLoginResult.Pass, loginResponse.Status, loginResponse.Message);
            }
        }
    }
}
