using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using PayPoint.Service;

namespace PayPoint.API.Config
{
    public class AutofacWebApi
    {
        public static void Setup(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            //register ApiControllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //Register Service(WebSitesService)
            builder.RegisterType<WebSitesService>()
                .As<IWebSitesService>()
                .InstancePerRequest();
            //Set dependency resolver
            config.DependencyResolver =new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}
