using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Routing.Constraints;

namespace PayPoint.API
{
    public class SiteidRouteConstraint : IHttpRouteConstraint
    {
        //private int _shouldSiteid = 10001;

        public bool Match(System.Net.Http.HttpRequestMessage request, IHttpRoute route, string parameterName,
                                        IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (values[parameterName] != RouteParameter.Optional)
            {
               // object value;
               // values.TryGetValue("siteid", out value);
                if (values.ContainsKey("siteid"))
                    return Convert.ToString(values["siteid"]) == "10001" ? true : false;
            }
            return true;
        }
    }
}
