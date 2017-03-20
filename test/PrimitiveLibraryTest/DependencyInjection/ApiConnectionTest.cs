using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.DependencyInjection;

namespace PrimitiveLibraryTest.DependencyInjection
{
    [TestClass]
    public class ApiConnectionTest
    {
        [TestMethod]
        public void TestFactoryInitialize()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection("https://zh.moegirl.org/api.php");
            Assert.AreEqual(false, apiFactory.m_hasThirdPartySessionProvider);
        }

        [TestMethod]
        public void TestCreateConnectionInstance()
        {
            var apiFactory = ApiConnectionFactory.CreateConnection("https://zh.moegirl.org/api.php");
            using (var apiConnection = apiFactory.CreateConnection())
            {
                var sessionFactory = apiConnection.CreateSessionFactory();
                var actionPipe = apiConnection.CreateActionPipeline();
            }
        }
    }
}
