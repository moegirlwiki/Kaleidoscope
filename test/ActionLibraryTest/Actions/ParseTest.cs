using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Actions;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ActionLibraryTest.Actions
{
    [TestClass]
    public class ParseTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public async Task TestParse()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {
                var queryResponse = await apiConnection.CreateAction<ParseAction>().RunActionAsync(config =>
                {
                    config.Page = "Mainpage";
                    config.AddParseProp(Moegirlpedia.MediaWikiInterop.Actions.Models.ParseProp.Text);

                }, session);

                Assert.AreEqual("Mainpage", queryResponse.Title);
                var content = queryResponse.GetPrimitiveTypedField<string>(Moegirlpedia.MediaWikiInterop.Actions.Models.ParseProp.Text);

                Assert.IsNotNull(content);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestParseInvalidArg()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {
                var queryResponse = await apiConnection.CreateAction<ParseAction>().RunActionAsync(config => { }, session);
            }
        }
    }
}
