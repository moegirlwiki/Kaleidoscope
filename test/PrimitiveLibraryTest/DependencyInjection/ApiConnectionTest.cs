//-----------------------------------------------------------------------
// <copyright file="ApiConnectionTest.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimitiveLibraryTest.DependencyInjection
{
    [TestClass]
    public class ApiConnectionTest
    {
        private const string ApiEndpoint = "https://zh.moegirl.org/api.php";

        [TestMethod]
        public void TestFactoryInitialize()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            Assert.AreEqual(false, apiFactory.m_hasThirdPartySessionProvider);
        }

        [TestMethod]
        public void TestCreateSession()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            {
                Assert.AreEqual(ApiEndpoint, session.m_requestEndpoint);
                Assert.IsNotNull(session.m_cookieContainer);
            }
        }

        [TestMethod]
        public async Task TestDefaultSessionIgnoreProviderActions()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var session = apiConnection.CreateSession())
            using (var httpPostContent = new StringContent(""))
            {
                Assert.AreEqual(ApiEndpoint, session.m_requestEndpoint);
                Assert.IsNotNull(session.m_cookieContainer);

                await session.SetCookieAsync();
                await session.SetHeaderAsync(httpPostContent);
            }
        }
    }
}
