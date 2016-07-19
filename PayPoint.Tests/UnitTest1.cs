using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayPoint.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string url = "http://localhost:51425/api/ppamselfserviceapi/1234/site/10001/summary";

        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void TestMethod1()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(new HttpMethod("GET"), url);
            var routeData = config.Routes.GetRouteData(request);

        }
    }
}
