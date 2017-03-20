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
        public void TestCreateConnectionInstance()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var actionPipe = apiConnection.CreateActionPipeline();
            }
        }

        [TestMethod]
        public void TestCreateSession()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var session = sessionFactory.CreateSession();

                Assert.AreEqual(ApiEndpoint, session.m_requestEndpoint);
                Assert.IsNotNull(session.m_cookieContainer);
            }
        }

        [TestMethod]
        public async Task TestDefaultSessionIgnoreProviderActions()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection(ApiEndpoint);
            using (var apiConnection = apiFactory.CreateConnection())
            using (var httpPostContent = new StringContent(""))
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var session = sessionFactory.CreateSession();

                Assert.AreEqual(ApiEndpoint, session.m_requestEndpoint);
                Assert.IsNotNull(session.m_cookieContainer);

                await session.SetCookieAsync();
                await session.SetHeaderAsync(httpPostContent);
            }
        }
    }
}
