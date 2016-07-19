using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PayPoint.API.Config
{
    public class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            var routes = config.Routes;

            //QUESTION 4
            //** IN ORDER TO BE SATISFIED ABOVE ROUTE SHOULD BE THE FOLLOWINGS
            //1. I have set controller name as constraint: Should be ALWAYS "site"
            //2. I have set siteid parameter value as constraint: Should be ALWAYS "100001"
            routes.MapHttpRoute(
                "ExampleHttpRoute1",
                "api/ppamselfserviceapi/1234/{controller}/{siteid}/summary",
                defaults: new {},
                constraints: new { controller = "Site", siteid=new SiteidRouteConstraint() }
                );




        }
    }
}
