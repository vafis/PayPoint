using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PayPoint.Domain;
using PayPoint.ViewModel;


namespace PayPoint.Service
{
    public class WebSitesService : IWebSitesService
    {
        //QUESTION 2: Task Parallel Library and async-await
        //introduced in .net 4.0 but in .Net 4.5 is more easier

        public async Task<WebSiteOwnerViewModel> GetWebSiteOwnerViewModel(int siteid)
        {
            Task<WebSite> task1 = this.GetWebSiteInfoAsync(siteid);
            Task<WebSiteOwner> task2 = this.GetWebSiteOwnerAsync();
          
            WebSiteOwnerViewModel webSiteOwnerViewModel = new WebSiteOwnerViewModel();
            await  Task.Factory.ContinueWhenAll(new Task[]{task1, task2}, (tasks) =>
            {
                webSiteOwnerViewModel.WebSite = task1.Result;
                webSiteOwnerViewModel.WebSiteOwner = task2.Result;
            });
            return webSiteOwnerViewModel;
        }

        public Task<WebSite> GetWebSiteInfoAsync(int siteid)
        {
            return Task<WebSite>.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return new WebSite() { Id = siteid, Url = "www.paypoint.com" };
            });
        }

        public Task<WebSiteOwner> GetWebSiteOwnerAsync()
        {
            
            return Task<WebSiteOwner>.Factory.StartNew(() =>
            {
               Thread.Sleep(9000);
                return new WebSiteOwner() {OwnerName = "Kostas Vafeiadakis", City = "Liverpool", Country = "UK"};
            });
        }
    }

    public interface IWebSitesService
    {
        Task<WebSiteOwnerViewModel> GetWebSiteOwnerViewModel(int siteid);
        Task<WebSite> GetWebSiteInfoAsync (int siteid);
        Task<WebSiteOwner> GetWebSiteOwnerAsync();
    }
}
