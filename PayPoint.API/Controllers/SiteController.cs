using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using PayPoint.Domain;
using PayPoint.Service;
using PayPoint.ViewModel;

namespace PayPoint.API.Controllers
{
    public class SiteController : ApiController
    {
        private IWebSitesService _webSitesService;

        public SiteController(IWebSitesService webSitesService)
        {
            _webSitesService = webSitesService;
        }

        public async Task<HttpResponseMessage> GetSite(int siteid)
        {
            //It's not necessary the following validation because its managed by route
            if (siteid != 10001)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            WebSiteOwnerViewModel webSiteOwnerViewMode = await _webSitesService.GetWebSiteOwnerViewModel(siteid);
                
            //QUESTION 5
            // return Json content
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<WebSiteOwnerViewModel>(webSiteOwnerViewMode, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };

        }

    }
}
