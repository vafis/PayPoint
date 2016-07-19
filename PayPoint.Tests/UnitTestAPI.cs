using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Moq;
using Newtonsoft.Json.Linq;
using PayPoint.API;
using PayPoint.API.Config;
using PayPoint.API.Controllers;
using PayPoint.Domain;
using PayPoint.Service;
using PayPoint.ViewModel;
using Xunit;
using Xunit.Extensions;

namespace PayPoint.Tests
{

    public class UnitTestApi
    {
        [Theory]
        [InlineData("http://localhost:51425/api/ppamselfserviceapi/1234/site/10002/summary", "site", "10002", false)]
        [InlineData("http://localhost:51425/api/ppamselfserviceapi/1234/anothersite/10001/summary", "anothersitesite", "10001", false)]
        [InlineData("http://localhost:51425/api/ppamselfserviceapi/1234/site/10001/summary", "site", "10001", true)]
        public void Routes_TEST_(string url, string controller, string siteid, bool exists)
        {
            var config = new HttpConfiguration();
            RouteConfig.RegisterRoutes(config);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            // act
            var routeData = config.Routes.GetRouteData(request);
            Assert.Equal(exists , routeData != null);
            if (exists)
            {
                Assert.Equal(controller, routeData.Values["controller"]);
                Assert.Equal(siteid , routeData.Values["siteid"]);
            }
        }

        [Theory]
        [InlineData("http://localhost:51425/api/ppamselfserviceapi/1234/site/10001/summary","site", 10001)]
        public void SiteController_GET_Test(string url, string controller, int siteid)
        {
            WebSiteOwnerViewModel viewModel = new WebSiteOwnerViewModel()
            {
                WebSite =new WebSite() {Id = 10001, Url = "http://localhost:51425/api/ppamselfserviceapi/1234/site/10001/summary"},
                WebSiteOwner = new WebSiteOwner() {City = "Liverpool", Country = "UK", OwnerName = "Kostas Vafeiadakis"}
            };

           
            var mock = new Mock<IWebSitesService>();
            mock.Setup(x=>x.GetWebSiteOwnerViewModel(It.IsAny<int>())).Returns(Task<WebSiteOwnerViewModel>.Factory.StartNew(()=>
            {
                return viewModel;
            }));
            
            var siteController = new SiteController(mock.Object);
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            var config = new HttpConfiguration();
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            siteController.Request = request;
            siteController.Configuration = config;
            config.Routes.MapHttpRoute(
                "ExampleHttpRoute1",
                "api/ppamselfserviceapi/1234/{controller}/{siteid}/summary",
                defaults: new {},
                constraints: new { controller = "Site", siteid = new SiteidRouteConstraint() }
                );
            var route = config.Routes["ExampleHttpRoute1"];
            var httpRouteData = new HttpRouteData(route, new HttpRouteValueDictionary(new { controller = controller, siteid = siteid }));
            siteController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = httpRouteData;

            var result =  siteController.GetSite(siteid).Result;
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }


        //QUESTION 5
        [Theory]
        [InlineData("http://localhost:51425/api/ppamselfserviceapi/1234/site/10001/summary", 10001, "Liverpool")]
        public async void  Site_controller_Integration_Test(string url, int siteid, string city)
        {
            var config = new HttpConfiguration();
            RouteConfig.RegisterRoutes(config);
            AutofacWebApi.Setup(config);
            var httpServer = new HttpServer(config);
            
            var client = HttpClientFactory.Create(innerHandler: httpServer);
            //var response = client.GetAsync(new Uri(url)).Result;
            HttpResponseMessage response = await client.GetAsync(new Uri(url));
            

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            WebSiteOwnerViewModel viewModel = json.ToObject<WebSiteOwnerViewModel>();
            Assert.Equal(siteid, viewModel.WebSite.Id);
            Assert.Equal(city,viewModel.WebSiteOwner.City );

        }

        [Fact]
        public void Test()
        {
            Assert.Equal(1,1);
        }
    }
}
